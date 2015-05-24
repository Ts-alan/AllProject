using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CCP.DAL.DataModels;
using CCP.DAL.Interfaces;
using CCP.WebApi.Interfaces;
using CCP.WebApi.Models;

namespace CCP.WebApi.Services
{
    public class GenericService<TEntity>: IService<TEntity>
    {
        private readonly IRepository<TEntity> _repository;

        public GenericService(IRepository<TEntity> repository )
        {
            _repository = repository;
        }

        protected GenericService()
        {
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> otherActions = null)
        {
            return _repository.Get(filter, orderBy, otherActions);
        }

        TEntity IService<TEntity>.GetById(object id)
        {
            return _repository.GetById(id);
        }

        public void Insert(TEntity entity)
        {
            _repository.Insert(entity);
            _repository.Save();
        }

        void IService<TEntity>.Delete(object id)
        {
            _repository.Delete(id);
            _repository.Save();
        }

        void IService<TEntity>.Delete(TEntity entityToDelete)
        {
            _repository.Delete(entityToDelete);
            _repository.Save();
        }

        public void Update(TEntity entityToUpdate)
        {
            _repository.Update(entityToUpdate);
            _repository.Save();
        }

       
    }
}