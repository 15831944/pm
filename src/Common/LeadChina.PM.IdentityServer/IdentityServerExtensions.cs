using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeadChina.PM.IdentityServer.Interfaces.Repositories;
using LeadChina.PM.IdentityServer.Interfaces.Services;
using LeadChina.PM.IdentityServer.Repositories.ClientAggregate.InMemory;
using LeadChina.PM.IdentityServer.Repositories.ResourceAggregate.InMemory;
using LeadChina.PM.IdentityServer.Repositories.UserAggregate.InMemory;
using LeadChina.PM.IdentityServer.Services;

namespace LeadChina.PM.IdentityServer
{
    public static class IdentityServerExtensions
    {
        /// <summary>
        /// 添加微服务身份认证模块
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddNanoFabricIDS(this IIdentityServerBuilder builder, IConfigurationRoot config)
        {
            var option = builder.Services.ConfigurePOCO(config.GetSection("IdentityOptions"), () => new IdentityOptions());
            builder.Services.AddTransient<IUserRepository, UserInMemoryRepository>();
            builder.Services.AddTransient<IResourceRepository, ResourceInMemoryRepository>();
            builder.Services.AddTransient<IClientRepository, ClientInMemoryRepository>();
            builder.Services.AddTransient<IClientStore, ClientInMemoryRepository>();
            builder.Services.AddTransient<IResourceStore, ResourceInMemoryRepository>();

            builder.AddOperationalStore(options =>
             {
                 options.RedisConnectionString = option.Redis;
                 options.KeyPrefix = option.KeyPrefix;
             })
             // 添加Redis缓存 
             .AddRedisCaching(options =>
             {
                 options.RedisConnectionString = option.Redis;
             });
            // services
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();
            // validators
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            builder.AddProfileService<ProfileService>();
            return builder;
        }

        public static IIdentityServerBuilder AddNanoFabricIdentityIDS(this IIdentityServerBuilder builder, IConfigurationRoot config)
        {
            var option = builder.Services.ConfigurePOCO(config.GetSection("IdentityOptions"), () => new IdentityOptions());
 
            builder.AddOperationalStore(options =>
            {
                options.RedisConnectionString = option.Redis;
                options.KeyPrefix = option.KeyPrefix;
            })
             .AddRedisCaching(options =>
             {
                 options.RedisConnectionString = option.Redis;
             });
 
            builder.AddProfileService<ProfileService>();

            return builder;
        }
    }
}
