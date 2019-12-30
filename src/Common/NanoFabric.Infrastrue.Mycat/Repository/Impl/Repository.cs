using LeadChina.ProjectManager.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    /// <summary>
    /// 泛型仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : RootEntity<TKey>
    {
        protected readonly LeadChinaPMDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork.DbContext as LeadChinaPMDbContext;
            DbSet = Db.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }

        public virtual TEntity Add(TEntity obj, bool IsCommit = true)
        {
            TEntity entity = DbSet.Add(obj).Entity;
            if (IsCommit) _unitOfWork.Commit();
            return entity;
        }

        public void AddRange(IEnumerable<TEntity> objs, bool IsCommit = true)
        {
            DbSet.AddRange(objs);
            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> objs, bool IsCommit = true)
        {
            await DbSet.AddRangeAsync(objs);
            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetAllAsTracking()
        {
            return DbSet.AsTracking();
        }

        public virtual void Update(TEntity obj, bool IsCommit = true)
        {
            //DbSet.Update(obj);
            DbSet.Attach(obj);
            Db.Entry(obj).State = EntityState.Modified;

            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual void UpdateRange(IEnumerable<TEntity> objs, bool IsCommit = true)
        {
            DbSet.AttachRange(objs);
            foreach (var obj in objs)
            {
                Db.Entry(obj).State = EntityState.Modified;
            }
            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual void UpdateRangeAsTracking(IEnumerable<TEntity> objs, bool IsCommit = true)
        {
            DbSet.UpdateRange(objs);
            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual void Remove(TKey id, bool IsCommit = true)
        {
            DbSet.Remove(DbSet.Find(id));
            if (IsCommit) _unitOfWork.Commit();
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities, bool IsCommit = true)
        {
            DbSet.RemoveRange(entities);
            if (IsCommit) _unitOfWork.Commit();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public TEntity FindOneAsNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public void Remove(TEntity obj, bool IsCommit = true)
        {
            DbSet.Remove(obj);
            if (IsCommit) _unitOfWork.Commit();
        }
    }
}
