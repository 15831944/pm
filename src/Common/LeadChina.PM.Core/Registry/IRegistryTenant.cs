﻿using System;

namespace LeadChina.PM.Core
{
    /// <summary>
    /// 注册Web API服务实例
    /// </summary>
    public interface IRegistryTenant
    {
        Uri Uri { get; }
    }
}
