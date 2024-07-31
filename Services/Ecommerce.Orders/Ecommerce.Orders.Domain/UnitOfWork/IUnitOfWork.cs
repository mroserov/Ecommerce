using Ecommerce.Orders.Domain.Repositories;

namespace Ecommerce.Orders.Domain.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IOrderRepository OrderRepository { get; }
    Task<int> CommitAsync();
}
