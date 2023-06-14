using System.Linq.Expressions;

namespace FA.JustBlog.Core.Infrastructures
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(int key);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);

        TEntity GetById(int key);

        IEnumerable<TEntity> Find(Func<TEntity, bool> condition);
    }
}