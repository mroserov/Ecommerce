using Ecommerce.Basket.Domain.Entities;

namespace Ecommerce.Basket.Domain.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart> GetByUserIdAsync(string userId);
    Task AddAsync(ShoppingCart cart);
    Task UpdateAsync(ShoppingCart cart);
    Task DeleteAsync(string userId);
}