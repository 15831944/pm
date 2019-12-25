using NanoFabric.Router.Consul;
using NanoFabric.Router.Throttle;

namespace NanoFabric.Router
{
    /// <summary>
    /// 服务订阅工厂
    /// </summary>
    public interface IServiceSubscriberFactory
    {
        IPollingServiceSubscriber CreateSubscriber(string serviceName);

        IPollingServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions,
            ThrottleSubscriberOptions throttleOptions);
    }
}
