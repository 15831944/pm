using NanoFabric.IdentityServer.Models;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository
    {
        Task<User> GetAsync(string username);
        Task<User> GetAsync(string username, string password);
        Task AddAsync(User entity, string password);
        Task DeleteAsync(int id);
        Task UpdateAsync(User entity);
        Task<User> GetAsync(int id);
    }
}
