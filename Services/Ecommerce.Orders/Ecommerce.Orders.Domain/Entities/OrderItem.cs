using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Orders.Domain.Entities;
public class OrderItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
    public string? Slug { get; set; }
    public Order Order { get; set; }
    public int OrderId { get; set; }
}
