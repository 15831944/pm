using AutoMapper;
using LeadChina.ProjectManager.SysSetting.BusiProcess;
using LeadChina.ProjectManager.SysSetting.BusiProcess.Impl;
using LeadChina.ProjectManager.SysSetting.BusiProcess.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.IdentityServer;
using NanoFabric.Infrastrue.Mycat;
using NanoFabric.Infrastrue.Mycat.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyWalking.AspNetCore;
using System.Text;

namespace LeadChina.ProjectManager.Identity
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // 注册一个编码提供者
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // 设置配置文件
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddMvc()
               // 添加JSON配置模块
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               });
            // 添加Consul服务注册模块
            services.AddNanoFabricConsul(Configuration);
            // AddIdentityServer方法在依赖注入系统中注册IdentityServer，它还会注册一个基于内存存储的运行时状态，
            // 这对于开发场景非常有用，对于生产场景，您需要一个持久化或共享存储，如数据库或缓存
            services.AddIdentityServer()
                // AddDeveloperSigningCredential扩展在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥
                //.AddDeveloperSigningCredential()
                .AddNanoFabricIDS(Configuration);

            // Automapper 注入
            // 添加服务
            services.AddAutoMapper();
            // 启动配置
            AutoMapperConfig.RegisterMappings();
            // .NET Core 原生依赖注入
            // 单写一层用来添加依赖项，从展示层 Presentation 中隔离
            services.AddDbContext<LeadChinaPMDbContext>();
            services.AddScoped<LeadChinaPMDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            // 添加跨域配置
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader();
                    corsBuilder.AllowAnyMethod();
                    corsBuilder.AllowAnyOrigin();
                    corsBuilder.AllowCredentials();
                });
            });

            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            services.AddSkyWalking(option =>
            {
                option.DirectServers = collectorUrl;
                option.ApplicationCode = "SampleService_Idserver";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            app.UseConsulRegisterService(Configuration);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
