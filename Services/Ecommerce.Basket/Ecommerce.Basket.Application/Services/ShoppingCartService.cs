using AutoMapper;
using Ecommerce.Basket.Domain.Entities;
using Ecommerce.Basket.Domain.Interfaces;

namespace Ecommerce.Basket.Application.Services;
public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _repository;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ShoppingCart> GetShoppingCartAsync(string userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task AddItemToShoppingCartAsync(string userId, ShoppingCartItem item)
    {
        var cart = await _repository.GetByUserIdAsync(userId) ?? new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem>() };
        var existingItem = cart.Items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
            existingItem.Price = item.Price;
            existingItem.Stock = item.Stock;
            existingItem.Discount = item.Discount;
            existingItem.Slug = item.Slug;
            existingItem.ImageUrl = item.ImageUrl;
        }
        else
        {
            cart.Items.Add(item);
        }

        await _repository.AddAsync(cart);
    }

    public async Task UpdateItemInShoppingCartAsync(string userId, ShoppingCartItem item)
    {
        var cart = await _repository.GetByUserIdAsync(userId);
        var existingItem = cart?.Items?.Find(x => x.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Quantity = item.Quantity;
            existingItem.Price = item.Price;
            await _repository.UpdateAsync(cart);
        }
    }

    public async Task RemoveItemFromShoppingCartAsync(string userId, string productId)
    {
        var cart = await _repository.GetByUserIdAsync(userId);
        var itemToRemove = cart?.Items?.Find(x => x.Id == productId);
        if (itemToRemove != null)
        {
            cart.Items.Remove(itemToRemove);
            await _repository.UpdateAsync(cart);
        }
    }

    public async Task ClearShoppingCartAsync(string userId)
    {
        var cart = await _repository.GetByUserIdAsync(userId);
        if (cart != null)
        {
            cart.Items.Clear();
            await _repository.UpdateAsync(cart);
        }
    }
}