using IdentityModel;
using LeadChina.ProjectManager.SysSetting.API.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NanoFabric.AspNetCore;
using NanoFabric.Infrastrue.Mycat;
using NanoFabric.Router;
using SkyWalking.AspNetCore;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeadChina.ProjectManager.SysSetting.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // 清除jwt声明
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加框架服务
            services.AddMvc();
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();

            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            services.AddSkyWalking(option =>
            {
                option.DirectServers = collectorUrl;
                option.ApplicationCode = "SampleService_MvcClient";
            });
            services.AddSingleton(p => new HttpClient());
            var authority = Configuration.GetValue<string>("Authority");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
           .AddCookie(options =>
           {
               options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
               options.Cookie.Name = "mvchybrid";
           })
           // 使用 OpenId Connect 添加用户身份认证
           .AddOpenIdConnect("oidc", options =>
           {
               options.Authority = authority;
               options.RequireHttpsMetadata = false;

               options.ClientSecret = "secret";
               options.ClientId = "mvc.hybrid";

               options.ResponseType = "code id_token";

               options.Scope.Clear();
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               //options.Scope.Add("email");
               options.Scope.Add("api1");
               //options.Scope.Add("offline_access");

               options.ClaimActions.Remove("amr");
               options.ClaimActions.MapJsonKey("website", "website");

               options.GetClaimsFromUserInfoEndpoint = true;
               options.SaveTokens = true;

               // Map here the claims for name and role 
               options.TokenValidationParameters =
                  new TokenValidationParameters
                  {
                      NameClaimType = JwtClaimTypes.Email,
                      RoleClaimType = JwtClaimTypes.Role,
                  };

               options.Events.OnRedirectToIdentityProvider = context =>
               {
                   context.ProtocolMessage.SetParameter("culture", CultureInfo.CurrentUICulture.Name);
                   return Task.FromResult(0);
               };

               options.Events.OnTicketReceived = async context =>
               {

               };
               // Automapper 注入
               services.AddAutoMapperSetup();
               // .NET Core 原生依赖注入
               // 单写一层用来添加依赖项，从展示层 Presentation 中隔离
               NativeInjectorBootStrapper.RegisterServices(services);
               services.AddDbContext<LeadChinaPMDbContext>();
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseConsulRegisterService(Configuration);
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
