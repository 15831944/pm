using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router
{
    /// <summary>
    /// 服务订阅者接口
    /// </summary>
    public  interface IServiceSubscriber : IDisposable
    {
        /// <summary>
        /// 获取注册信息
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default);
    }
}
