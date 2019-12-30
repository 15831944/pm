using LeadChina.ProjectManager.SysSetting.API.Extensions;
using LeadChina.ProjectManager.SysSetting.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NanoFabric.AspNetCore;
using NanoFabric.Autofac;
using NanoFabric.Core;
using NanoFabric.Infrastrue.Mycat;
using NanoFabric.Mediatr;
using NanoFabric.Mediatr.Autofac;
using NanoFabric.Router;
using NanoFabric.Swagger;
using SkyWalking.AspNetCore;
using System;

namespace LeadChina.ProjectManager.SysSetting.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // 添加框架服务
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();

            services.AddCors();
            services.AddDistributedMemoryCache();

            services
                .AddSingleton<IRequestManager, InMemoryRequestManager>()
                .AddSingleton(ApiInfo.Instantiate(Configuration))
                .AddPermissiveCors()
                .AddCustomIdentity(ApiInfo.Instance)
                .AddCustomSwagger(ApiInfo.Instance);

            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            services.AddSkyWalking(option =>
            {
                option.DirectServers = collectorUrl;
                option.ApplicationCode = "SampleService_MvcClient";
            });

            services.AddMvc().AddMvcApiResult();

            // Automapper 注入
            services.AddAutoMapperSetup();
            // .NET Core 原生依赖注入
            // 单写一层用来添加依赖项，从展示层 Presentation 中隔离
            NativeInjectorBootStrapper.RegisterServices(services);
            services.AddDbContext<LeadChinaPMDbContext>();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            return services.ConvertToAutofac(MediatrModule.Create(ApiInfo.Instance.ApplicationAssembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApiInfo apiInfo)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseDeveloperExceptionPage()
                .UsePermissiveCors()
                .UseCustomSwagger(apiInfo)
                .UseAuthentication()
                .UseMvc()
                .UseStaticFiles()
                .UseConsulRegisterService(Configuration);
        }
    }
}
