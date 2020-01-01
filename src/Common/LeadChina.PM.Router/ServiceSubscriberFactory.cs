using LeadChina.PM.Router.Cache;
using LeadChina.PM.Router.Consul;
using LeadChina.PM.Router.Throttle;

namespace LeadChina.PM.Router
{
    /// <summary>
    /// 服务订阅工厂
    /// </summary>
    public class ServiceSubscriberFactory : IServiceSubscriberFactory
    {
        private readonly IConsulServiceSubscriberFactory _consulServiceSubscriberFactory;
        private readonly ICacheServiceSubscriberFactory _cacheServiceSubscriberFactory;

        public ServiceSubscriberFactory(IConsulServiceSubscriberFactory consulServiceSubscriberFactory, 
            ICacheServiceSubscriberFactory cacheServiceSubscriberFactory)
        {
            _consulServiceSubscriberFactory = consulServiceSubscriberFactory;
            _cacheServiceSubscriberFactory = cacheServiceSubscriberFactory;
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="servicName"></param>
        /// <returns></returns>
        public IPollingServiceSubscriber CreateSubscriber(string servicName)
        {
            return CreateSubscriber(servicName, ConsulSubscriberOptions.Default, ThrottleSubscriberOptions.Default);
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="consulOptions"></param>
        /// <param name="throttleOptions"></param>
        /// <returns></returns>
        public IPollingServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, ThrottleSubscriberOptions throttleOptions)
        {
            // 创建Consul服务订阅者
            var consulSubscriber = _consulServiceSubscriberFactory.CreateSubscriber(serviceName, consulOptions, true);
            // 创建限流服务订阅者
            var throttleSubscriber = new ThrottleServiceSubscriber(consulSubscriber, throttleOptions.MaxUpdatesPerPeriod, throttleOptions.MaxUpdatesPeriod);
            return _cacheServiceSubscriberFactory.CreateSubscriber(throttleSubscriber);
        }
    }
}
