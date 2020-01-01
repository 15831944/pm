using LeadChina.PM.SysSetting.Application;
using LeadChina.PM.SysSetting.Infrastrue;
using LeadChina.PM.SysSetting.Infrastrue.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LeadChina.PM.SysSetting.API.Extensions
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
