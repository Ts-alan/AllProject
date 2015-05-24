using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CCP.WebApi.Interfaces
{
    public interface IService<TEntity>
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> otherActions = null   
            );
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}