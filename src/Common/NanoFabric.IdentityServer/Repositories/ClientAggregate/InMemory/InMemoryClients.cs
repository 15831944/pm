using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using static IdentityServer4.IdentityServerConstants;

namespace NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public class InMemoryClients
    {
        private IConfigurationRoot configuration;

        public InMemoryClients()
        {
            // 加载配置文件
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static IEnumerable<Client> Clients 
        {
            get {
                return new List<Client>
                {
                    new Client
                    {
                        ClientId = "mvc.hybrid",
                        ClientSecrets = { new Secret("secret".Sha256()) },
                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                        // 登录成功回调处理地址，处理回调返回的数据
                        RedirectUris = { "http://localhost:9000/signin-oidc" },
                        AllowedScopes = {
                            StandardScopes.OpenId,
                            StandardScopes.Profile,
                            StandardScopes.OfflineAccess,
                            "api1"
                        },
                        // AllowOfflineAccess 允许我们通过刷新令牌的方式来实现长期的 API 访问
                        AllowOfflineAccess = true
                    }
                };
            }
        }
    }
}
