using Ecommerce.Catalog.Domain.Entities;
using Ecommerce.Catalog.Domain.Interfaces;
using Ecommerce.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Catalog.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CatalogDbContext _context;

    public CategoryRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
    }

    public void DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
    }

    public Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds)
    {
        var categories = _context.Categories.AsQueryable();
        return categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
    }
}
