using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Catalog.Domain.Dtos;

public class ProductDto : ProductImageDto
{
    [Required]
    [MinLength(3)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    public decimal Discount { get; set; } = 0;

    public string? Slug { get; set; }

    public List<int> CategoryIds { get; set; }
}
