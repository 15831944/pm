using Microsoft.EntityFrameworkCore;
using System;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    /// <summary>
    /// 工作单元类
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(LeadChinaPMDbContext context)
        {
            DbContext = context;
        }

        public DbContext DbContext { get; }

        /// <summary>
        /// 上下文提交
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            return DbContext.SaveChanges() > 0;
        }

        ~UnitOfWork()
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
            DbContext.Dispose();
        }
    }
}
