namespace LeadChina.PM.RegistryHost.ConsulRegistry
{
    /// <summary>
    /// Consul服务发现选项
    /// </summary>
    public class ConsulServiceDiscoveryOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Consul服务端
        /// </summary>
        public ConsulRegistryHostConfiguration Consul { get; set; }

        /// <summary>
        /// 健康检查路由
        /// </summary>
        public string HealthCheckTemplate { get; set; }

        /// <summary>
        /// 端点
        /// </summary>
        public string[] Endpoints { get; set; }
    }
}
