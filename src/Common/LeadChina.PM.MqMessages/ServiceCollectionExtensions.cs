using LeadChina.PM.Core.MqMessages;
using LeadChina.PM.MqMessages.RebusCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeadChina.PM.MqMessages
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMqMessages(this IServiceCollection services)
        {
            services.AddSingleton<IMqMessagePublisher, RebusRabbitMqPublisher>();
            return services;
        }
    }
}
