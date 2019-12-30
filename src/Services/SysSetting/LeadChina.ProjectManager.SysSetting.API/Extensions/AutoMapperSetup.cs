using AutoMapper;
using LeadChina.ProjectManager.SysSetting.BusiProcess.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LeadChina.ProjectManager.SysSetting.API.Extensions
{
    /// <summary>
    /// AutoMapper 的启动服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 添加服务
            services.AddAutoMapper();
            // 启动配置
            AutoMapperConfig.RegisterMappings();
        }
    }
}
