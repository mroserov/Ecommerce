using Ecommerce.Basket.Domain.Interfaces;

namespace Ecommerce.Basket.Domain.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IShoppingCartRepository Baskets { get; }
    Task<int> CompleteAsync();
}