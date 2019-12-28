using NanoFabric.IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Services
{
    public interface IUserService 
    {
        Task<User> GetAsync(string username);
        Task<User> GetAsync(string username, string password);
        Task AddAsync(User entity, string password);
        Task<User> GetAsync(int id);
        Task<User> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims);

        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<bool> ValidateCredentialsAsync(string username, string password);

        Task<User> FindByExternalProviderAsync(string provider, string userId);
    }
}
