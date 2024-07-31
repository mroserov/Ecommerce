using Ecommerce.Orders.Domain.Repositories;
using Ecommerce.Orders.Domain.UnitOfWork;
using Ecommerce.Orders.Infrastructure.Data;
using Ecommerce.Orders.Infrastructure.Repositories;

namespace Ecommerce.Orders.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _context;
    private IOrderRepository _orderRepository;

    public UnitOfWork(OrderDbContext context)
    {
        _context = context;
    }

    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
