using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;

namespace Ecommerce.Catalog.Api.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class CategoryQueries
{
    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync([Service] ICategoryService categoryService)
    {
        return await categoryService.GetAllCategories();
    }
}
