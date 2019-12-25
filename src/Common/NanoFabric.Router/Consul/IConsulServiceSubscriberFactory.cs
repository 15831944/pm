namespace NanoFabric.Router.Consul
{
    /// <summary>
    /// Consul服务订阅工厂
    /// </summary>
    public interface IConsulServiceSubscriberFactory
    {
        IServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, bool watch);
    }
}
