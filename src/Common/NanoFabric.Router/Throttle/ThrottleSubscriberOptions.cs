using System;

namespace NanoFabric.Router.Throttle
{
    /// <summary>
    /// 限流订阅者选项
    /// </summary>
    public class ThrottleSubscriberOptions
    {
        public static readonly ThrottleSubscriberOptions Default = new ThrottleSubscriberOptions();

        /// <summary>
        /// 每个周期内最大更新数，默认5个
        /// </summary>
        public int MaxUpdatesPerPeriod { get; set; } = 5;

        /// <summary>
        /// 最大更新周期，默认10s
        /// </summary>
        public TimeSpan MaxUpdatesPeriod { get; set; } = TimeSpan.FromSeconds(10);
    }
}