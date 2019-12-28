using NanoFabric.Router.Cache.Internal;

namespace NanoFabric.Router.Cache
{
    /// <summary>
    /// 缓存服务订阅者工厂
    /// </summary>
    public class CacheServiceSubscriberFactory : ICacheServiceSubscriberFactory
    {
        private readonly ICacheClient _cacheClient;

        public CacheServiceSubscriberFactory(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceSubscriber"></param>
        /// <returns></returns>
        public IPollingServiceSubscriber CreateSubscriber(IServiceSubscriber serviceSubscriber)
        {
            return new CacheServiceSubscriber(serviceSubscriber, _cacheClient);
        }
    }
}
