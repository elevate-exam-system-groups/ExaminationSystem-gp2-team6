using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared;
using ExaminationSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ExaminationSystem.Infrastructure.Persistence.Repositories
{
    public class GenericRepository
    {
    }
}

namespace Hotel.Persistence.Repositories
{
    public class GenericRepository<T, TKey>(AppDbContext _context) : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        public IQueryable<T> GetAll()
        {
            var result = _context.Set<T>().AsQueryable();
            return result;
        }

        public IQueryable<T?> GetById(TKey id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.Id!.Equals(id));
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public void Add(T entity)
        {
             _context.Set<T>().Add(entity);
        }

        public void Update(T entity, params string[] modifiedParams)
        {
            var local = _context.Set<T>().Local
                .FirstOrDefault(x => x.Id!.Equals(entity.Id));

            EntityEntry entry;
            if (local == null)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.Entry(local);
            }

            foreach (var propName in modifiedParams)
            {
                var value = entity.GetType()
                                  .GetProperty(propName)!
                                  .GetValue(entity);

                entry.Property(propName).CurrentValue = value;
                entry.Property(propName).IsModified = true;
            }
        }

        public void SoftDelete(TKey id)
        {
            var local = _context.Set<T>().Local
                .FirstOrDefault(x => x.Id!.Equals(id));

            EntityEntry entry;
            if (local == null)
            {
                var entity = Activator.CreateInstance<T>(); // Create a new instance of T 
                entity.Id = id;

                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.Entry(local);
            }
            entry.Property(nameof(BaseEntity<TKey>.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(BaseEntity<TKey>.IsDeleted)).IsModified = true;

        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
    }
}
