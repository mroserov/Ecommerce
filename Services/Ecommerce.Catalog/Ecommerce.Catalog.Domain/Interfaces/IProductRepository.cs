using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;

namespace Ecommerce.Catalog.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product> GetProductById(int id);
    Task AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<PagedResultDto<Product>> GetProducts(string? searchTerm, int pageNumber, int pageSize);
}
