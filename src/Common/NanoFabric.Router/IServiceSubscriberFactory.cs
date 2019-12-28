using NanoFabric.Router.Consul;
using NanoFabric.Router.Throttle;

namespace NanoFabric.Router
{
    /// <summary>
    /// 服务订阅工厂
    /// </summary>
    public interface IServiceSubscriberFactory
    {
        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        IPollingServiceSubscriber CreateSubscriber(string serviceName);

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="consulOptions">consul配置选项</param>
        /// <param name="throttleOptions">限流订阅者选项</param>
        /// <returns></returns>
        IPollingServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions,
            ThrottleSubscriberOptions throttleOptions);
    }
}
