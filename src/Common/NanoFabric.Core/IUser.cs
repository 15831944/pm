using System.Collections.Generic;
using System.Security.Claims;

namespace NanoFabric.Core
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUser
    {
        string Id { get; }
        string Name { get; }
        IEnumerable<Claim> Claims { get; }
    }
}
