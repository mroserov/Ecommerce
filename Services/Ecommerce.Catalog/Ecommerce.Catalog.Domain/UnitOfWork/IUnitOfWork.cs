using Ecommerce.Catalog.Domain.Interfaces;

namespace Ecommerce.Catalog.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }
    Task<int> CompleteAsync();
}
