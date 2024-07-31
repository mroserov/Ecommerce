using Ecommerce.Catalog.Domain.Entities;

namespace Ecommerce.Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task<Category?> GetCategoryById(int id);
    Task AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);
    Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds);
}
