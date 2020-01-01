using LeadChina.PM.Core;
using LeadChina.PM.Router.Cache.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadChina.PM.Router.Cache
{
    /// <summary>
    /// 缓存服务订阅者
    /// </summary>
    public class CacheServiceSubscriber : IPollingServiceSubscriber
    {
        /// <summary>
        /// 是否释放标识
        /// </summary>
        private bool _disposed;

        private readonly ICacheClient _cache;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly IServiceSubscriber _serviceSubscriber;

        /// <summary>
        /// 用作缓存的key，唯一生成码
        /// </summary>
        private readonly string _id = Guid.NewGuid().ToString();

        private Task _subscriptionTask;
        
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);

        public event EventHandler EndpointsChanged;

        public CacheServiceSubscriber(IServiceSubscriber serviceSubscriber, ICacheClient cache)
        {
            _cache = cache;
            _serviceSubscriber = serviceSubscriber;
        }

        /// <summary>
        /// 获取服务注册信息
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CacheServiceSubscriber));
            }

            await StartSubscription(ct).ConfigureAwait(false);

            return _cache.Get<List<RegistryInformation>>(_id);
        }

        /// <summary>
        /// 启动订阅
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task StartSubscription(CancellationToken ct = default)
        {
            // 没有创建订阅任务时，创建一个
            if (_subscriptionTask == null)
            {
                await _mutex.WaitAsync(ct).ConfigureAwait(false);
                try
                {
                    if (_subscriptionTask == null)
                    {
                        // 获取服务注册信息
                        var serviceUris = await _serviceSubscriber.Endpoints(ct).ConfigureAwait(false);
                        // 设置缓存
                        _cache.Set(_id, serviceUris);
                        // 开启任务
                        _subscriptionTask = StartSubscriptionLoop(serviceUris);
                    }
                }
                finally
                {
                    _mutex.Release();
                }
            }
        }

        /// <summary>
        /// 启动订阅轮询
        /// </summary>
        /// <param name="previousEndpoints"></param>
        /// <returns></returns>
        private Task StartSubscriptionLoop(List<RegistryInformation> previousEndpoints)
        {
            return Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var currentEndpoints = await _serviceSubscriber.Endpoints(_cts.Token).ConfigureAwait(false);
                        // 服务注册信息不匹配时
                        if (!EndpointListsMatch(previousEndpoints, currentEndpoints))
                        {
                            // 更新缓存
                            _cache.Set(_id, currentEndpoints);
                            // 触发端口变更事件
                            EndpointsChanged?.Invoke(this, EventArgs.Empty);
                            previousEndpoints = currentEndpoints;
                        }
                    }
                    catch(Exception ex)
                    {
                        // ignore

                    }
                }
            }, _cts.Token);
        }

        /// <summary>
        /// 判断注册信息是否匹配
        /// </summary>
        /// <param name="endpoints1"></param>
        /// <param name="endpoints2"></param>
        /// <returns></returns>
        private static bool EndpointListsMatch(ICollection<RegistryInformation> endpoints1, ICollection<RegistryInformation> endpoints2)
        {
            if (endpoints1.Count != endpoints2.Count)
            {
                return false;
            }

            var filteredSequence = endpoints1.Where(endpoints2.Contains);
            return filteredSequence.Count() == endpoints1.Count;
        }

        ~CacheServiceSubscriber()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // 资源释放
            if (disposing)
            {
                if (!_cts.IsCancellationRequested)
                {
                    _cts.Cancel();
                }
                _cts.Dispose();
                _mutex.Dispose();
                _serviceSubscriber.Dispose();
            }

            // 删除缓存
            _cache.Remove(_id);
            // 更新标识
            _disposed = true;
        }
    }
}
