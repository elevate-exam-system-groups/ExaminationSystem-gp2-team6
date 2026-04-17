using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IGenericRepository<T, TKey>
    {
        IQueryable<T> GetAll();
        IQueryable<T?> GetById(TKey id, params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        void Update(T entity, params string[] modifiedParams);
        void SoftDelete(TKey id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    }
}
