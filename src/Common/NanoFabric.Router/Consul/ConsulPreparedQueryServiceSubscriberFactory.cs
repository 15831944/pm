using Consul;

namespace NanoFabric.Router.Consul
{
    /// <summary>
    /// Consul预查询服务订阅工厂
    /// </summary>
    public class ConsulPreparedQueryServiceSubscriberFactory : IConsulPreparedQueryServiceSubscriberFactory
    {
        private readonly IConsulClient _consulClient;

        public ConsulPreparedQueryServiceSubscriberFactory(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public IServiceSubscriber CreateSubscriber(string queryName)
        {
            return new ConsulPreparedQueryServiceSubscriber(_consulClient, queryName);
        }
    }
}
