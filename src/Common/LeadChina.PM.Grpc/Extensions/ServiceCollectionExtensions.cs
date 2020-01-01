using LeadChina.PM.Grpc.Client;
using Microsoft.Extensions.DependencyInjection;

namespace LeadChina.PM.Grpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcClient(this IServiceCollection services)
        {
            services.AddSingleton<IGRpcConnection, GRpcConnection>();
            services.AddSingleton<IGrpcChannelFactory, GrpcChannelFactory>();
            return services;
        }
    }
}
