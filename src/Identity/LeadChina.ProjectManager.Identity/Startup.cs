using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NanoFabric.Core.Json;
using Newtonsoft.Json;
using System.Text;

namespace LeadChina.ProjectManager.Identity
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment env)
        {
            // 注册一个编码提供者
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _environment = env;
            // 设置配置文件
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
