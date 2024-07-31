using Ecommerce.Catalog.Domain.Interfaces;
using Ecommerce.Catalog.Domain.UnitOfWork;
using Ecommerce.Catalog.Infrastructure.Data;
using Ecommerce.Catalog.Infrastructure.Repositories;

namespace Ecommerce.Catalog.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CatalogDbContext _context;
    private IProductRepository _productRepository;
    private ICategoryRepository _categoryRepository;

    public UnitOfWork(CatalogDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(_context);

    public ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
