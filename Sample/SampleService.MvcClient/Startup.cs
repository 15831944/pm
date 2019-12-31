using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NanoFabric.AspNetCore;
using NanoFabric.Router;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleService.MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // 关闭了 JWT Claim 类型映射，以允许常用的Claim（例如'sub'和'idp'）
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();
            //var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            //services.AddSkyWalking(option =>
            //{
            //    option.DirectServers = collectorUrl;
            //    option.ApplicationCode = "SampleService_MvcClient";
            //});
            services.AddSingleton(p => new HttpClient());
            var authority = Configuration.GetValue<string>("Authority");
            // AddAuthentication将身份认证服务添加到 DI
            services.AddAuthentication(options =>
            {
                // 使用 cookie 来本地登录用户（通过 "Cookies" 作为 DefaultScheme）
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // 将 DefaultChallengeScheme 设置为 "oidc"，因为当我们需要用户登录时，我们将使用 OpenID Connect 协议
                options.DefaultChallengeScheme = "oidc";
            })
           // 使用 AddCookie 添加可以处理 cookie 的处理程序
           .AddCookie(options =>
           {
               options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
               options.Cookie.Name = "mvchybrid";
           })
           // AddOpenIdConnect 用于配置执行 OpenID Connect 协议的处理程序
           .AddOpenIdConnect("oidc", options =>
           {
               // Authority 表明我们信任的 IdentityServer 地址
               options.Authority = authority;
               options.RequireHttpsMetadata = false;

               options.ClientSecret = "secret";
               // 我们通过 ClientId 识别这个客户端
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

               // SaveTokens 用于在 cookie 中保留来自 IdentityServer 的令牌
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
           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            // 确保认证服务执行对每个请求的验证，加入 UseAuthentication 到 Configure 中
            // 应在管道中的 MVC 之前添加认证中间件
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
/**
 * 最后一步是触发身份认证。为此，请转到 Controller 并添加 [Authorize] 特性到其中一个 Action
 */
