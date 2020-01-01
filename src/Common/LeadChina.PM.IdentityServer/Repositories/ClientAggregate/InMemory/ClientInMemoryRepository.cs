using IdentityServer4.Models;
using LeadChina.PM.IdentityServer.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadChina.PM.IdentityServer.Repositories.ClientAggregate.InMemory
{
    /// <summary>
    /// 客户端仓储
    /// </summary>
    public class ClientInMemoryRepository : IClientRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return InMemoryClients.Clients.SingleOrDefault(x => x.ClientId == clientId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetAllAllowedCorsOriginsAsync()
        {
            var origins = InMemoryClients.Clients.SelectMany(x => x.AllowedCorsOrigins);
            return origins;
        }
    }
}
