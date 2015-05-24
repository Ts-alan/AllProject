using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CCP.DAL.Interfaces;

namespace CCP.DAL
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        IQueryable<TEntity> IRepository<TEntity>.Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> otherActions)
        {
            var query = dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (otherActions != null)
            {
                query = otherActions(query);
            }
            return query;
        }

        TEntity IRepository<TEntity>.GetById(object id)
        {
            return dbSet.Find(id);
        }

        void IRepository<TEntity>.Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        void IRepository<TEntity>.Delete(object id)
        {
            var entityToDelete = (TEntity) dbSet.Find(id);
            ((IRepository<TEntity>) this).Delete(entityToDelete);
        }

        void IRepository<TEntity>.Delete(TEntity entityToDelete)
        {
            if (context.Entry((object) entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        void IRepository<TEntity>.Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry((object) entityToUpdate).State = EntityState.Modified;
        }

        void IRepository<TEntity>.Save()
        {
            context.SaveChanges();
        }
    }
}
