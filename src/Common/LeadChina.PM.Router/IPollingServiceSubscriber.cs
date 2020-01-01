using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeadChina.PM.Router
{
    /// <summary>
    /// 轮询服务订阅接口
    /// </summary>
    public  interface IPollingServiceSubscriber : IServiceSubscriber
    {
        /// <summary>
        /// 启动订阅
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task StartSubscription(CancellationToken ct = default);

        /// <summary>
        /// 端点变化事件
        /// </summary>
        event EventHandler EndpointsChanged;
    }
}
