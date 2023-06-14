using FA.JustBlog.Core.Data;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FA.JustBlog.Core.Infrastructures
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly JustBlogContext context;
        protected DbSet<TEntity> dbSet;

        public GenericRepository(JustBlogContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public virtual void Delete(int key)
        {
            var entity = dbSet.Find(key);
            if (entity == null) return;
            Delete(entity);
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> condition)
        {
            return this.dbSet.Where(condition);
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public virtual TEntity GetById(int key)
        {
            return this.dbSet.Find(key);
        }

        public virtual void Update(TEntity entity)
        {
            context.Entry<TEntity>(entity).State = EntityState.Modified;
        }
    }
}