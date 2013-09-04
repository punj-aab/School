using StudentTracker.Core.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity.Migrations;
namespace StudentTracker.Core.Repositories
{
    /// <summary>
    ///  A Generic Class for CRUD operations on Entities.
    ///  Inherited from IRepository interface.
    /// </summary>
    public class Repository : IRepository, IDisposable
    {
        protected readonly StudentContext context;
        private bool disposed = false;
        private readonly string connectionString = string.Empty;
        
        public Repository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["GatewayContext"].ConnectionString;
            context = context ?? (context = new StudentContext(connectionString));
        }

        public Repository(string conn)
        {
            context = context ?? (context = new StudentContext(conn));
        }

        public TEntity Get<TEntity>(long id) where TEntity : class
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Get<TEntity>(string id) where TEntity : class
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Get<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return context.Set<TEntity>().Single(whereClause);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetMany<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            return context.Set<TEntity>().Where(whereClause);
        }

        public IQueryable<TEntity> GetWithoutProxy<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            using (StudentContext context = new StudentContext(connectionString))
            {
                context.Configuration.ProxyCreationEnabled = false;
                return context.Set<TEntity>().Where<TEntity>(whereClause);
            }
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : class
        {
            return context.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            context.Set<TEntity>();
            context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifier, params TEntity[] entities) where TEntity : class
        {
            context.Set<TEntity>().AddOrUpdate(identifier, entities);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Attach(entity);
            context.Set<TEntity>().Remove(entity);
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            foreach (var item in GetMany<TEntity>(whereClause))
            {
                Delete(item);
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
