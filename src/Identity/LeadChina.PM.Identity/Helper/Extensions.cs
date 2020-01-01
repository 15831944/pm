using IdentityServer4.Stores;
using System.Threading.Tasks;

namespace LeadChina.PM.Identity.Helper
{
    public static class Extensions
    {
        /// <summary>
        /// 决定客户端是否配置了使用PKCE
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="client_id">The client identifier.</param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string client_id)
        {
            if (!string.IsNullOrWhiteSpace(client_id))
            {
                var client = await store.FindEnabledClientByIdAsync(client_id);
                return client?.RequirePkce == true;
            }
            return false;
        }
    }
}
