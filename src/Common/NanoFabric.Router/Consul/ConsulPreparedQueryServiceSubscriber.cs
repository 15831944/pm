using Consul;
using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router.Consul
{
    /// <summary>
    /// Consul预查询服务订阅者
    /// </summary>
    public class ConsulPreparedQueryServiceSubscriber : IServiceSubscriber
    {
        private readonly IConsulClient _client;
        private readonly string _queryName;

        public ConsulPreparedQueryServiceSubscriber(IConsulClient client, string queryName)
        {
            _client = client;
            _queryName = queryName;
        }

        /// <summary>
        /// 获取查询的注册信息
        /// </summary>
        /// <param name="ct">取消通知</param>
        /// <returns></returns>
        public async Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default)
        {
            var servicesQuery = await
               _client.PreparedQuery.Execute(_queryName, ct)
                   .ConfigureAwait(false);

            return servicesQuery.Response.Nodes.Select(service => service.ToEndpoint()).ToList();
        }

        /// <summary>
        /// 释放资源
        /// 实现IDisposable，此方法不能是虚拟的，派生类不应重写此方法。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // 从终结队列中去除，以防止此对象的终结代码再次执行。
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _client.Dispose();
        }

        ~ConsulPreparedQueryServiceSubscriber()
        {
            Dispose(false);
        }
    }
}