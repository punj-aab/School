using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace StudentTracker.Core.Repositories
{
    /// <summary>
    /// A Generic Interface for CRUD operations on Entities.
    /// Implemented in Repository class
    /// </summary>
    public interface IRepository
    {
        TEntity Get<TEntity>(long id) where TEntity : class;

        TEntity Get<TEntity>(string id) where TEntity : class;

        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

        IQueryable<TEntity> GetMany<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        IQueryable<TEntity> GetWithoutProxy<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        TEntity Add<TEntity>(TEntity entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifier, params TEntity[] entities) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;

        void SaveChanges();

        void Dispose();
    }
}
