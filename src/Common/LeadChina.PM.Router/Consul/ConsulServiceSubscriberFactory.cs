using Consul;

namespace LeadChina.PM.Router.Consul
{
    /// <summary>
    /// Consul服务订阅工厂
    /// </summary>
    public class ConsulServiceSubscriberFactory : IConsulServiceSubscriberFactory
    {
        private readonly IConsulClient _consulClient;

        public ConsulServiceSubscriberFactory(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="consulOptions">Consul订阅者选项</param>
        /// <param name="watch"></param>
        /// <returns></returns>
        public IServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, bool watch = false)
        {
            return new ConsulServiceSubscriber(_consulClient, serviceName, consulOptions, watch);
        }
    }
}
