namespace LeadChina.PM.Router.Consul
{
    /// <summary>
    /// Consul预查询服务订阅工厂
    /// </summary>
    public interface IConsulPreparedQueryServiceSubscriberFactory
    {
        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        IServiceSubscriber CreateSubscriber(string queryName);
    }
}
