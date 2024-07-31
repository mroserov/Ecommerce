using Ecommerce.Common.Entities;
using System.Linq.Expressions;

namespace Ecommerce.Common.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    IQueryable<T> GetAll();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
