using LeadChina.PM.Core;
using System.Threading;
using System.Threading.Tasks;

namespace LeadChina.PM.Router
{
    /// <summary>
    /// 负载均衡接口
    /// </summary>
    public interface ILoadBalancer
    {
        /// <summary>
        /// 获取服务注册信息
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<RegistryInformation> Endpoint(CancellationToken ct = default);
    }
}
