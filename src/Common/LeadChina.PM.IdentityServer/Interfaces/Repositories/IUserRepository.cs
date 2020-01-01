using LeadChina.PM.IdentityServer.Models;
using System.Threading.Tasks;

namespace LeadChina.PM.IdentityServer.Interfaces.Repositories
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetAsync(string username);

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<User> GetAsync(string username, string password);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task AddAsync(User entity, string password);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(User entity);

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetAsync(int id);
    }
}
