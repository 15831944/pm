using LeadChina.ProjectManager.SysSetting.BusiProcess;
using LeadChina.ProjectManager.SysSetting.BusiProcess.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Infrastrue.Mycat;
using NanoFabric.Infrastrue.Mycat.Repository;

namespace LeadChina.ProjectManager.SysSetting.API.Extensions
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<LeadChinaPMDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IOrgRepository, OrgRepository>();

            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
