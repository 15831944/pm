using System;
using System.Net;

namespace LeadChina.PM.RegistryHost.ConsulRegistry
{
    /// <summary>
    /// Consul服务端配置
    /// </summary>
    public class ConsulRegistryHostConfiguration
    {
        /// <summary>
        /// url路径
        /// </summary>
        public string HttpEndpoint { get; set; }

        /// <summary>
        /// 域名路径
        /// </summary>
        public DnsEndpoint DnsEndpoint { get; set; }

        /// <summary>
        /// 数据中心
        /// </summary>
        public string Datacenter { get; set; }

        public string AclToken { get; set; }

        public TimeSpan? LongPollMaxWait { get; set; }

        public TimeSpan? RetryDelay { get; set; } = Defaults.ErrorRetryInterval;
    }

    /// <summary>
    /// 域名服务端
    /// </summary>
    public class DnsEndpoint
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取包含地址和端口的网络节点对象
        /// </summary>
        /// <returns></returns>
        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }

    public static class Defaults
    {
        public static TimeSpan ErrorRetryInterval => TimeSpan.FromSeconds(15);

        public static TimeSpan UpdateMaxInterval => TimeSpan.FromSeconds(15);
    }
}
