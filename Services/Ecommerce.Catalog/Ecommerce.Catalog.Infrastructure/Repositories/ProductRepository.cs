using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;
using Ecommerce.Catalog.Domain.Interfaces;
using Ecommerce.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _context;

    public ProductRepository(CatalogDbContext context)
    {
        _context = context;
    }
    public async Task<PagedResultDto<Product>> GetProducts(string? searchTerm, int pageNumber, int pageSize)
    {
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = _context.Products.Where(p => p.Name.Contains(searchTerm) || (p.Description != null && p.Description.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<Product>()
        {
            Items = products,
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber
        };
    }
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products.Include(p => p.Categories).ToListAsync();
    }

    public async Task<Product> GetProductById(int id)
    {
        return await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Slug))
        {
            product.Slug = product.Name.ToLower().Replace(' ', '_');
        }

        await _context.Products.AddAsync(product);
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }
}
