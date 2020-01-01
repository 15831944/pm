using System.Collections.Generic;

namespace LeadChina.PM.SysSetting.Application
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IService<TEntity, TKey>
    {
        /// <summary>
        /// 查询全部记录
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(TKey id);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        void Save(TEntity entity);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(TKey id);
    }
}
