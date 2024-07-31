namespace Ecommerce.Basket.Domain.Entities;
public class ShoppingCart
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public List<ShoppingCartItem>? Items { get; set; }
}