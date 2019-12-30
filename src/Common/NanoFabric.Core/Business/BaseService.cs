using AutoMapper;
using LeadChina.ProjectManager.Entity;
using NanoFabric.Infrastrue.Mycat.Repository;
using System.Collections.Generic;

namespace NanoFabric.Core
{
    /// <summary>
    /// 基础服务抽象类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TViewModel, TEntity, TKey> where TEntity : RootEntity<TKey>
    {
        private readonly IMapper _mapper;

        protected abstract IRepository<TEntity, TKey> Repository { get; }

        /// <summary>
        /// 构造器注入
        /// </summary>
        /// <param name="mapper"></param>
        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<TViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<TViewModel>>(Repository.GetAll());
        }

        public TViewModel GetById(TKey id)
        {
            return _mapper.Map<TViewModel>(Repository.GetById(id));
        }

        public void Remove(TKey id)
        {
            Repository.Remove(id);
        }

        public void Save(TViewModel viewModel)
        {
            var dataEntity = _mapper.Map<TEntity>(viewModel);
            if (dataEntity.Id.Equals(0))
            {
                Repository.Add(dataEntity);
            }
            else
            {
                Repository.Update(dataEntity);
            }
        }
    }
}
