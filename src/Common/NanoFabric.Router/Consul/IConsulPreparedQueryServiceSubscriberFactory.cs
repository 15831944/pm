namespace NanoFabric.Router.Consul
{
    public interface IConsulPreparedQueryServiceSubscriberFactory
    {
        IServiceSubscriber CreateSubscriber(string queryName);
    }
}
