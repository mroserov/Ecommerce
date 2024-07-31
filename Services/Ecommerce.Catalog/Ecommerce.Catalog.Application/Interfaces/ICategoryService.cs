using Ecommerce.Catalog.Domain.Dtos;

namespace Ecommerce.Catalog.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategories();
        Task<CategoryResponseDto> GetCategoryById(int id);
        Task<CategoryResponseDto> AddCategory(CategoryDto categoryDto);
        Task UpdateCategory(int id, CategoryDto categoryDto);
        Task DeleteCategory(int id);
        Task<List<CategoryResponseDto>> GetCategoriesByIdsAsync(List<int> categoryIds);
    }
}
