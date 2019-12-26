using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory
{
    /// <summary>
    /// 内存中客户端
    /// </summary>
    public class InMemoryClients
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
            {
                    ClientId = "mvc.hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RedirectUris = { "http://localhost:9000/signin-oidc" },
                    AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.OfflineAccess,
                    "api1"
                }
            }
        };
    }
}
