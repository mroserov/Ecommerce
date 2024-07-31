namespace Ecommerce.Catalog.Domain.Dtos;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public decimal Discount { get; set; }
    public string? Slug { get; set; }
    public List<CategoryResponseDto> Categories { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
