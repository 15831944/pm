using IdentityServer4.Models;
using NanoFabric.IdentityServer.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory
{
    /// <summary>
    /// 客户端仓储
    /// </summary>
    public class ClientInMemoryRepository : IClientRepository
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return InMemoryClients.Clients.SingleOrDefault(x => x.ClientId == clientId);
        }

        public async Task<IEnumerable<string>> GetAllAllowedCorsOriginsAsync()
        {
            var origins = InMemoryClients.Clients.SelectMany(x => x.AllowedCorsOrigins);
            return origins;
        }
    }
}
