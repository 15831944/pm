using Consul;
using LeadChina.PM.Core;
using LeadChina.PM.RegistryHost.ConsulRegistry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadChina.PM.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 微服务注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app, IConfiguration configuration)
        {
            ConsulServiceDiscoveryOption serviceDiscoveryOption = new ConsulServiceDiscoveryOption();
            configuration.GetSection("ServiceDiscovery").Bind(serviceDiscoveryOption);
            app.UseConsulRegisterService(serviceDiscoveryOption);
            return app;
        }
        /// <summary>
        /// 应用启动时注册微服务生成服务代理，应用结束时注销
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceDiscoveryOption"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app, ConsulServiceDiscoveryOption serviceDiscoveryOption)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>() ??
               throw new ArgumentException("依赖丢失", nameof(IApplicationLifetime));
            if (serviceDiscoveryOption.Consul == null)
                throw new ArgumentException("依赖丢失", nameof(serviceDiscoveryOption.Consul));
            var consul = app.ApplicationServices.GetRequiredService<IConsulClient>() ?? throw new ArgumentException("依赖丢失", nameof(IConsulClient));

            // 创建日志对象记录重要信息
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("NanoFabricServiceBuilder");

            if (string.IsNullOrEmpty(serviceDiscoveryOption.ServiceName))
            {
                throw new ArgumentException("服务名称必须配置", nameof(serviceDiscoveryOption.ServiceName));
            }
            IEnumerable<Uri> addresses = null;
            if (serviceDiscoveryOption.Endpoints != null && serviceDiscoveryOption.Endpoints.Length > 0)
            {
                logger.LogInformation($"服务注册时使用 {serviceDiscoveryOption.Endpoints.Length} 个配置的端点");
                addresses = serviceDiscoveryOption.Endpoints.Select(p => new Uri(p));
            }
            else
            {
                logger.LogInformation($"尝试在注册时使用服务的特性（server.Features）得到服务端口信息");
                var features = app.Properties["server.Features"] as FeatureCollection;
                addresses = features.Get<IServerAddressesFeature>().Addresses.Select(p => new Uri(p)).ToArray();
            }

            foreach (var address in addresses)
            {
                var serviceID = GetServiceId(serviceDiscoveryOption.ServiceName, address);
                logger.LogInformation($"为地址 {address} 注册服务 {serviceID}");
                Uri healthCheck = null;
                if (!string.IsNullOrEmpty(serviceDiscoveryOption.HealthCheckTemplate))
                {
                    healthCheck = new Uri(address, serviceDiscoveryOption.HealthCheckTemplate);
                    logger.LogInformation($"为 {serviceID} 添加健康检查，验证 {healthCheck}");
                }
                var registryInformation = app.AddTenant(serviceDiscoveryOption.ServiceName, serviceDiscoveryOption.Version, address, healthCheckUri: healthCheck, tags: new[] { $"urlprefix-/{serviceDiscoveryOption.ServiceName}" });
                logger.LogInformation("正在注册额外的健康检查");
                // 应用程序停止时注销微服务和对应的健康检查服务
                applicationLifetime.ApplicationStopping.Register(() =>
                {
                    logger.LogInformation("注销微服务和对应健康检查服务");
                    app.RemoveTenant(registryInformation.Id);
                });
            }
            return app;
        }

        /// <summary>
        /// 获取服务Id（服务Id由服务名称和服务url地址以一定的规则拼接而成）
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetServiceId(string serviceName, Uri uri)
        {
            return $"WebAPI_{serviceName}_{uri.Host.Replace(".", "_")}_{uri.Port}";
        }

        public static RegistryInformation AddTenant(this IApplicationBuilder app, string serviceName, 
            string version, Uri uri, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            var registryInformation = serviceRegistry.RegisterServiceAsync(
                serviceName, version, uri, healthCheckUri, tags).Result;
            return registryInformation;
        }

        public static bool RemoveTenant(this IApplicationBuilder app, string serviceId)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (string.IsNullOrEmpty(serviceId))
            {
                throw new ArgumentNullException(nameof(serviceId));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            return serviceRegistry.DeregisterServiceAsync(serviceId)
                .Result;
        }

        public static string AddHealthCheck(this IApplicationBuilder app, RegistryInformation registryInformation, Uri checkUri, TimeSpan? interval = null, string notes = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (registryInformation == null)
            {
                throw new ArgumentNullException(nameof(registryInformation));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            string checkId = serviceRegistry.AddHealthCheckAsync(registryInformation.Name, registryInformation.Id, checkUri, interval, notes)
                .Result;

            return checkId;
        }

        public static bool RemoveHealthCheck(this IApplicationBuilder app, string checkId)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (string.IsNullOrEmpty(checkId))
            {
                throw new ArgumentNullException(nameof(checkId));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            return serviceRegistry.DeregisterHealthCheckAsync(checkId)
                .Result;
        }

        /// <summary>
        /// 跨域策略
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePermissiveCors(this IApplicationBuilder app)
            => app.UseCors("PermissiveCorsPolicy");
    }
}