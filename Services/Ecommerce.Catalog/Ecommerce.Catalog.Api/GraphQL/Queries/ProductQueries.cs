using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;

namespace Ecommerce.Catalog.Api.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class ProductQueries
{
    public async Task<ProductResponseDto> GetProductByIdAsync([Service] IProductService productService, int id)
    {
        return await productService.GetProductById(id);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync([Service] IProductService productService)
    {
        return await productService.GetAllProducts();
    }
    public async Task<PagedResultDto<ProductResponseDto>> GetProductsAsync(
        [Service] IProductService productService,
        string? searchTerm,
        int pageNumber = 1,
        int pageSize = 10)
    {
        return await productService.GetProductsAsync(searchTerm, pageNumber, pageSize);
    }
}
