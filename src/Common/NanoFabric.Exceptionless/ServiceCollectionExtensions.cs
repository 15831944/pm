using Microsoft.Extensions.DependencyInjection;

namespace NanoFabric.Exceptionless
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册分布式异常日志收集插件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNanoFabricExceptionless(this IServiceCollection services)
        {            
            services.AddSingleton<ILessLog, LessLog>();
            services.AddSingleton<ILessLinksLog, LessLinksLog>();
            services.AddSingleton<ILessFeatureLog, LessFeatureLog>();
            services.AddSingleton<ILessExceptionLog, LessExceptionLog>();

            return services;
        }
    }
}
