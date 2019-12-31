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
            // ע��һ�������ṩ��
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // ���������ļ�
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
               // ���JSON����ģ��
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               });
            // ���Consul����ע��ģ��
            services.AddNanoFabricConsul(Configuration);
            // AddIdentityServer����������ע��ϵͳ��ע��IdentityServer��������ע��һ�������ڴ�洢������ʱ״̬��
            // ����ڿ��������ǳ����ã�������������������Ҫһ���־û�����洢�������ݿ�򻺴�
            services.AddIdentityServer()
                // AddDeveloperSigningCredential��չ��ÿ������ʱ��Ϊ����ǩ��������һ����ʱ��Կ�������ɻ�����Ҫһ���־û�����Կ
                //.AddDeveloperSigningCredential()
                .AddNanoFabricIDS(Configuration);

            // Automapper ע��
            // ��ӷ���
            services.AddAutoMapper();
            // ��������
            AutoMapperConfig.RegisterMappings();
            // .NET Core ԭ������ע��
            // ��дһ����������������չʾ�� Presentation �и���
            services.AddDbContext<LeadChinaPMDbContext>();
            services.AddScoped<LeadChinaPMDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            // ��ӿ�������
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
