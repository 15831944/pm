using LeadChina.ProjectManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    /// <summary>
    /// 定义泛型仓储接口，并继承IDisposable，显式释放资源
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : RootEntity<TKey>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="obj"></param>
        TEntity Add(TEntity obj, bool IsCommit = true);

        /// <summary>
        /// 根据id获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(TKey id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsTracking();

        /// <summary>
        /// 根据对象进行更新
        /// </summary>
        /// <param name="obj"></param>
        void Update(TEntity obj, bool IsCommit = true);

        void UpdateRange(IEnumerable<TEntity> objs, bool IsCommit = true);

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(TKey id, bool IsCommit = true);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        TEntity FindOneAsNoTracking(Expression<Func<TEntity, bool>> predicate);

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        void AddRange(IEnumerable<TEntity> objs, bool IsCommit = true);

        Task AddRangeAsync(IEnumerable<TEntity> objs, bool IsCommit = true);

        void Remove(TEntity obj, bool IsCommit = true);

        void RemoveRange(IEnumerable<TEntity> entities, bool IsCommit = true);

        void UpdateRangeAsTracking(IEnumerable<TEntity> objs, bool IsCommit = true);
    }
}
