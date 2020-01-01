using Consul;
using DnsClient;
using LeadChina.PM.RegistryHost.ConsulRegistry;
using LeadChina.PM.Router.Cache;
using LeadChina.PM.Router.Cache.Internal;
using LeadChina.PM.Router.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace LeadChina.PM.Router
{
    /// <summary>
    /// 服务集扩展
    /// </summary>
    public static  class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加微服务路由
        /// </summary>
        /// <param name="services">服务集</param>
        /// <param name="configuration">服务配置</param>
        /// <returns></returns>
        public static IServiceCollection AddNanoFabricConsulRouter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            // "ServiceDiscovery"节点的服务配置生效
            services.Configure<ConsulServiceDiscoveryOption>(configuration.GetSection("ServiceDiscovery"));
            services.AddNanoFabricConsulRouter();
            return services;
        }

        /// <summary>
        /// 添加微服务路由
        /// </summary>
        /// <param name="services">服务集</param>
        /// <returns></returns>
        public static IServiceCollection AddNanoFabricConsulRouter(this IServiceCollection services)
        {
            // 注册Consul客户端
            services.RegisterConsulClient();
            // 添加域名查找客户端
            services.RegisterDnsLookup();
            // 注册Consul服务订阅工厂单例
            services.AddSingleton<IConsulServiceSubscriberFactory, ConsulServiceSubscriberFactory>();
            // 注册Consul预查询服务订阅工厂单例
            services.AddSingleton<IConsulPreparedQueryServiceSubscriberFactory, ConsulPreparedQueryServiceSubscriberFactory>();
            // 注册服务订阅工厂
            services.TryAddTransient<IServiceSubscriberFactory, ServiceSubscriberFactory>();
            // 注册缓存订阅者
            services.AddCacheServiceSubscriber();
            return services;
        }

        /// <summary>
        /// 注册缓存服务（非分布式）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheServiceSubscriber(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            // 添加缓存
            services.AddMemoryCache();
            // 添加缓存客户端单例
            services.TryAddSingleton<ICacheClient, CacheClient>();
            // 添加缓存服务订阅者工厂单例
            services.TryAddSingleton<ICacheServiceSubscriberFactory, CacheServiceSubscriberFactory>();
            return services;
        }

        /// <summary>
        /// 实现域名服务查找客户端，注册到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private  static IServiceCollection RegisterDnsLookup(this IServiceCollection services)
        {
            // 单例模式
            services.TryAddSingleton<IDnsQuery>(p =>
            {
                // 读取服务配置
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;
                // 生成查询客户端（默认地址和端口，下面会被配置项覆盖）
                var client = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                // 更新域名服务的地址和端口
                if (serviceConfiguration.Consul.DnsEndpoint != null)
                {
                    client = new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
                }
                // 如果启用，每个DnsClient.IDnsQueryResponse都将包含响应的完整文档，默认值为False。
                client.EnableAuditTrail = false;
                // 获取或设置一个标志，该标志指示DnsClient.LookupClient是否应使用响应缓存。
                // 缓存持续时间由响应的资源记录计算。通常，使用最低的TTL(Time To Live)。默认值为True。
                client.UseCache = true;
                // 获取或设置System.TimeSpan，它可以在记录的TTL低于此最小值时重写资源记录的TTL，默认值为空。
                // 这在服务器以零TTL重新运行记录的情况下非常有用。
                client.MinimumCacheTimeout = TimeSpan.FromSeconds(1);
                return client;
            });
            return services;
        }

        /// <summary>
        /// 实现Consul客户端，注册到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection RegisterConsulClient(this IServiceCollection services)
        {
            // 客户端使用单例
            services.TryAddSingleton<IConsulClient>(p => new ConsulClient(config =>
            {
                // 注册中心地址默认赋值（下面会被配置的地址覆盖）
                config.Address = new Uri("http://127.0.0.1:8500");
                // 读取服务配置
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;
                // 更新注册中心地址
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            return services;
        }
    }
}
