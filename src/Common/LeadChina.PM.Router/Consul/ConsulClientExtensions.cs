using Consul;
using LeadChina.PM.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeadChina.PM.Router.Consul
{
    /// <summary>
    /// Consul客户端扩展
    /// </summary>
    public static class ConsulClientExtensions
    {
        private const string VERSION_PREFIX = "version-";

        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <param name="serviceEntry">服务入口</param>
        /// <returns></returns>
        public static RegistryInformation ToEndpoint(this ServiceEntry serviceEntry)
        {
            // 服务地址
            var host = !string.IsNullOrWhiteSpace(serviceEntry.Service.Address)
                ? serviceEntry.Service.Address
                : serviceEntry.Node.Address;
            // 返回注册信息对象
            return new RegistryInformation
            {
                Name = serviceEntry.Service.Service,
                Address = host,
                Port = serviceEntry.Service.Port,
                Version = GetVersionFromStrings(serviceEntry.Service.Tags),
                Tags = serviceEntry.Service.Tags ?? Enumerable.Empty<string>(),
                Id = serviceEntry.Service.ID
            };
        }

        /// <summary>
        /// 根据标签获取版本
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        private  static string GetVersionFromStrings(IEnumerable<string> strings)
        {
            return strings
                ?.FirstOrDefault(x => x.StartsWith(VERSION_PREFIX, StringComparison.Ordinal))
                .TrimStart(VERSION_PREFIX);
        }
    }
}
