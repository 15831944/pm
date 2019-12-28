using System.Collections.Generic;

namespace NanoFabric.Router.Consul
{
    /// <summary>
    /// Consul订阅者选项
    /// </summary>
    public class ConsulSubscriberOptions
    {
        /// <summary>
        /// 默认选项
        /// </summary>
        public static readonly ConsulSubscriberOptions Default = new ConsulSubscriberOptions();

        /// <summary>
        /// 标签集合
        /// </summary>
        public List<string> Tags { get; set; }

        public bool PassingOnly { get; set; } = true;
    }
}