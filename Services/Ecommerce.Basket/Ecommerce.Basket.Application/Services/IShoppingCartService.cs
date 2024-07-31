using Ecommerce.Basket.Domain.Entities;

namespace Ecommerce.Basket.Application.Services;

public interface IShoppingCartService
{
    Task<ShoppingCart> GetShoppingCartAsync(string userId);
    Task AddItemToShoppingCartAsync(string userId, ShoppingCartItem item);
    Task UpdateItemInShoppingCartAsync(string userId, ShoppingCartItem item);
    Task RemoveItemFromShoppingCartAsync(string userId, string productId);
    Task ClearShoppingCartAsync(string userId);
}
