using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Basket.Domain.Entities;
public class ShoppingCartItem
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public decimal Discount { get; set; }
    public string? Slug { get; set; }
    public string? ImageUrl { get; set; }
}

