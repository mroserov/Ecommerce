using Ecommerce.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Common.Repositories;

public class Repository<TContext, T> : IRepository<T>
    where T : BaseEntity, new()
    where TContext : DbContext
{
    protected readonly TContext _context;
    private readonly DbSet<T> _entities;

    public Repository(TContext context)
    {
        _context = context;
        _entities = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public IQueryable<T> GetAll()
    {
        return _entities.AsQueryable();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
