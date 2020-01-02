using LeadChina.PM.IdentityServer.Models;
using NodaTime;
using System.Collections.Generic;

namespace LeadChina.PM.IdentityServer.Repositories.UserAggregate.InMemory
{
    /// <summary>
    /// 内存中示例用户
    /// </summary>
    public static class InMemoryUsers
    {
        public static List<User> Users = new List<User>
        {
            User.Hydrate(1, "23907", Instant.FromUtc(2017,1,24,6,6), true),
            User.Hydrate(2, "-00001", Instant.FromUtc(2017,1,24,6,6), true),
        };
    }
}
