using Consul;
using DnsClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NanoFabric.RegistryHost.ConsulRegistry;
using NanoFabric.Router.Cache;
using NanoFabric.Router.Cache.Internal;
using NanoFabric.Router.Consul;
using System;
using System.Net;

namespace NanoFabric.Router
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoFabricConsulRouter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ConsulServiceDiscoveryOption>(configuration.GetSection("ServiceDiscovery"));
            services.AddNanoFabricConsulRouter();
            return services;
        }

        /// <summary>
        /// 注册微服务路由
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNanoFabricConsulRouter(this IServiceCollection services)
        {
            services.RegisterConsulClient();
            services.RegisterDnsLookup();
            services.AddSingleton<IConsulServiceSubscriberFactory, ConsulServiceSubscriberFactory>();
            services.AddSingleton<IConsulPreparedQueryServiceSubscriberFactory, ConsulPreparedQueryServiceSubscriberFactory>();
            services.TryAddTransient<IServiceSubscriberFactory, ServiceSubscriberFactory>();
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
            services.AddMemoryCache();
            services.TryAddSingleton<ICacheClient, CacheClient>();
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
            services.TryAddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;

                var client = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                if (serviceConfiguration.Consul.DnsEndpoint != null)
                {
                    client = new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
                }
                client.EnableAuditTrail = false;
                client.UseCache = true;
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
            services.TryAddSingleton<IConsulClient>(p => new ConsulClient(config =>
            {
                config.Address = new Uri("http://127.0.0.1:8500");
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            return services;
        }
    }
}
