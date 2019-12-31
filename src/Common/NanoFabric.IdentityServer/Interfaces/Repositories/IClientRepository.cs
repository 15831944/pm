using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    /// <summary>
    /// 检索客户端配置
    /// </summary>
    public interface IClientRepository : IClientStore
    {
        Task<IEnumerable<string>> GetAllAllowedCorsOriginsAsync();
    }
}
