using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    public interface IClientRepository : IClientStore
    {
        Task<IEnumerable<string>> GetAllAllowedCorsOriginsAsync();
    }
}
