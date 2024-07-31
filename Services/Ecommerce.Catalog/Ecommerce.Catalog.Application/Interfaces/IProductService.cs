using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;

namespace Ecommerce.Catalog.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllProducts();
    Task<ProductResponseDto> GetProductById(int id);
    Task<ProductResponseDto> AddProduct(ProductDto product, ProductImage image);
    Task UpdateProduct(int id, ProductDto product, ProductImage image);
    Task DeleteProduct(int id);
    Task<PagedResultDto<ProductResponseDto>> GetProductsAsync(string? searchTerm, int pageNumber, int pageSize);
}
