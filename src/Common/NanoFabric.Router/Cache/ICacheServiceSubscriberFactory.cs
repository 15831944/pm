namespace NanoFabric.Router.Cache
{
    /// <summary>
    /// 缓存服务订阅工厂
    /// </summary>
    public interface ICacheServiceSubscriberFactory
    {
        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="serviceSubscriber"></param>
        /// <returns></returns>
        IPollingServiceSubscriber CreateSubscriber(IServiceSubscriber serviceSubscriber);
    }
}
