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
    /// Consul服务订阅者
    /// </summary>
    public class ConsulServiceSubscriber : IServiceSubscriber
    {
        private readonly IConsulClient _client;
        private readonly List<string> _tags;
        private readonly string _serviceName;
        private readonly bool _passingOnly;
        private readonly bool _watch;

        public ulong WaitIndex;

        public ConsulServiceSubscriber(IConsulClient client, string serviceName, 
            ConsulSubscriberOptions consulOptions, bool watch) : 
            this(client, serviceName, consulOptions.Tags, consulOptions.PassingOnly, watch)
        {

        }

        public ConsulServiceSubscriber(IConsulClient client, string serviceName, List<string> tags, 
            bool passingOnly, bool watch)
        {
            _client = client;

            _serviceName = serviceName;
            _tags = tags ?? new List<string>();
            _passingOnly = passingOnly;

            _watch = watch;
        }

        public async Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default(CancellationToken))
        {
            // Consul在其服务查询方法中不支持多个标记。
            // https://github.com/hashicorp/consul/issues/294
            // Hashicorp建议使用预查询（prepared queries），但它们不支持阻塞。
            // https://www.consul.io/docs/agent/http/query.html#execute
            // 如果我们想通过阻塞提高效率，就必须手动过滤标签。
            var tag = string.Empty;
            if (_tags.Count > 0)
            {
                tag = _tags[0];
            }

            var queryOptions = new QueryOptions
            {
                WaitIndex = WaitIndex
            };
            // 查询健康检查服务
            var servicesTask = await
                _client.Health.Service(_serviceName, tag, _passingOnly, queryOptions, ct)
                    .ConfigureAwait(false);
            // 根据标签过滤
            if (_tags.Count > 1)
            {
                servicesTask.Response = FilterByTag(servicesTask.Response, _tags);
            }
            // 
            if (_watch)
            {
                WaitIndex = servicesTask.LastIndex;
            }
            // 返回服务的注册信息
            return servicesTask.Response.Select(service => service.ToEndpoint()).ToList();
        }

        /// <summary>
        /// 根据标签过滤
        /// </summary>
        /// <param name="entries">服务入口集合</param>
        /// <param name="tags">只读标签集合</param>
        /// <returns></returns>
        private static ServiceEntry[] FilterByTag(IEnumerable<ServiceEntry> entries, IReadOnlyCollection<string> tags)
        {
            return entries
                .Where(x => tags.All(x.Service.Tags.Contains))
                .ToArray();
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

        ~ConsulServiceSubscriber()
        {
            Dispose(false);
        }
    }
}
