using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IGenericRepository<T, TKey> where T : class
    {
        IQueryable<T> GetAll(bool withNoTracking = true);
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}