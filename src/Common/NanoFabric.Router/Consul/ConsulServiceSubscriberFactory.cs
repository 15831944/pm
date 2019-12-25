using Consul;

namespace NanoFabric.Router.Consul
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

        public IServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, bool watch = false)
        {
            return new ConsulServiceSubscriber(_consulClient, serviceName, consulOptions, watch);
        }
    }
}
