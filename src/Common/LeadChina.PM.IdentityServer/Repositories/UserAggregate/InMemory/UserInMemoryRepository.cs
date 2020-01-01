using LeadChina.PM.IdentityServer.Interfaces.Repositories;
using LeadChina.PM.IdentityServer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LeadChina.PM.IdentityServer.Repositories.UserAggregate.InMemory
{
    /// <summary>
    /// 用户仓储类
    /// </summary>
    public class UserInMemoryRepository : IUserRepository
    {
        public Task AddAsync(User entity)
        {
            InMemoryUsers.Users.Add(entity);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task AddAsync(User entity, string password)
        {
            InMemoryUsers.Users.Add(entity);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(int id)
        {
            InMemoryUsers.Users.RemoveAll(x => x.Id == id);
            return Task.FromResult(0);
        }

        public Task<User> GetAsync(int id)
        {
            return Task.FromResult(InMemoryUsers.Users.SingleOrDefault(x => x.Id == id));
        }

        public Task<User> GetAsync(string username)
        {
            return Task.FromResult(InMemoryUsers.Users.SingleOrDefault(x => x.Username == username));
        }

        public Task<User> GetAsync(string username, string password)
        {
            return GetAsync(username);
        }

        public Task UpdateAsync(User entity)
        {
            DeleteAsync(entity.Id);
            AddAsync(entity);
            return Task.FromResult(0);
        }
    }
}
