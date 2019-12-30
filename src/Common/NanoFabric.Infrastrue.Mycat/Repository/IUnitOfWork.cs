using Microsoft.EntityFrameworkCore;
using System;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// 是否提交成功
        /// </summary>
        /// <returns></returns>
        bool Commit();
    }
}
