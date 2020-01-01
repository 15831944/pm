using Exceptionless;
using Exceptionless.Models;
using IdentityServer4.AccessTokenValidation;
using LeadChina.PM.AppMetrics;
using LeadChina.PM.AspNetCore;
using LeadChina.PM.Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.Administration;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using SkyWalking.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadChina.PM.Gateway.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetValue<string>("Authority");
            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            // 身份认证配置
            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authority;
                o.ApiName = "api";
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "secret";
            };
            var authenticationProviderKey = "apikey";
            Action<IdentityServerAuthenticationOptions> options2 = o =>
            {
                o.Authority = authority;
                o.ApiName = "api1";
                o.RequireHttpsMetadata = false;
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options2);

            services.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                 .AddConsul()
                 .AddConfigStoredInConsul()
                 .AddPolly()
                 .AddAdministration("/administration", options);

            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricExceptionless();

            services.AddAppMetrics(x =>
            {
                var opt = Configuration.GetSection("AppMetrics").Get<AppMetricsOptions>();
                x.App = opt.App;
                x.ConnectionString = opt.ConnectionString;
                x.DataBaseName = opt.DataBaseName;
                x.Env = opt.Env;
                x.Password = opt.Password;
                x.UserName = opt.UserName;
            });

            services.AddSkyWalking(option =>
            {
                option.DirectServers = collectorUrl;
                option.ApplicationCode = "nanofabric_ocelot";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionless(Configuration);
            loggerFactory.AddExceptionless();
            app.UseConsulRegisterService(Configuration);
            app.UseOcelot().Wait();
            app.UseAppMetrics();
            ExceptionlessClient.Default.SubmittingEvent += Default_SubmittingEvent;
        }

        /// <summary>
        /// 默认提交异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Default_SubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            var argEvent = e.Event;
            if (argEvent.Type == Event.KnownTypes.Log && argEvent.Source == "Ocelot.Configuration.Repository.FileConfigurationPoller")
            {
                e.Cancel = true;
                return;
            }
            // 只处理未处理的异常
            if (!e.IsUnhandledError)
                return;

            // 忽略没有错误体的错误
            var error = argEvent.GetError();
            if (error == null)
                return;

            // 忽略404错误
            if (e.Event.IsNotFound())
            {
                e.Cancel = true;
                return;
            }

            // 忽略401(Unauthorized)和请求验证的错误.
            if (error.Code == "401")
            {
                e.Cancel = true;
                return;
            }

            // 忽略任何未被代码抛出的异常
            var handledNamespaces = new List<string> { "Exceptionless" };
            var handledNamespaceList = error.StackTrace.Select(s => s.DeclaringNamespace).Distinct();
            if (!handledNamespaceList.Any(ns => handledNamespaces.Any(ns.Contains)))
            {
                e.Cancel = true;
                return;
            }
            // 添加系统异常标签
            e.Event.Tags.Add("未捕获异常");
            // 标记为关键异常
            e.Event.MarkAsCritical();
        }
    }
}
