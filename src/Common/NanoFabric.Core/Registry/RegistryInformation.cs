using System;
using System.Collections.Generic;

namespace NanoFabric.Core
{
    /// <summary>
    /// 服务注册信息
    /// </summary>
    public class RegistryInformation
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        public Uri ToUri(string scheme = "http", string path = "/")
        {
            var builder = new UriBuilder(scheme, Address, Port, path);
            return builder.Uri;
        }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}
