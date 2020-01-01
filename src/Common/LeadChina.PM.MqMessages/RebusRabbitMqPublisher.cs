using LeadChina.PM.Core.Json;
using LeadChina.PM.Core.MqMessages;
using LeadChina.PM.Core.Threading;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using System.Threading.Tasks;

namespace LeadChina.PM.MqMessages.RebusCore
{
    public class RebusRabbitMqPublisher : IMqMessagePublisher
    {
        private readonly IBus _bus;

        public ILogger Logger { get; set; }

        public RebusRabbitMqPublisher(IBus bus, ILoggerFactory factory)
        {
            _bus = bus;
            Logger = factory.CreateLogger<RebusRabbitMqPublisher>();
        }

        public void Publish(object mqMessages)
        {
            Logger.LogDebug(mqMessages.GetType().FullName + ":" + mqMessages.ToJsonString());

            AsyncHelper.RunSync(() => _bus.Publish(mqMessages));
        }

        public async Task PublishAsync(object mqMessages)
        {
            Logger.LogDebug(mqMessages.GetType().FullName + ":" + mqMessages.ToJsonString());

            await _bus.Publish(mqMessages);
        }
    }
}
