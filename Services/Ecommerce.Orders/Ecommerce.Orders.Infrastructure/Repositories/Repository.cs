using Ecommerce.Orders.Domain.Repositories;
using Ecommerce.Orders.Infrastructure.Data;

namespace Ecommerce.Orders.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly OrderDbContext _context;

    public Repository(OrderDbContext context)
    {
        _context = context;
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    // Otros métodos CRUD
}
