namespace NanoFabric.IdentityServer
{
    /// <summary>
    /// 身份认证配置项
    /// </summary>
    public class IdentityOptions
    {      
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string Redis { get; set; }

        /// <summary>
        /// 保存在Redis中的每个key的前缀项
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}