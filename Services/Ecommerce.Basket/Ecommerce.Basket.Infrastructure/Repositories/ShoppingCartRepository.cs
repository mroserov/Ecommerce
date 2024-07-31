using Ecommerce.Basket.Domain.Entities;
using Ecommerce.Basket.Domain.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ecommerce.Basket.Infrastructure.Repositories;
public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly IConnectionMultiplexer _redis;

    public ShoppingCartRepository(IConnectionMultiplexer redis)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
    }

    public async Task AddAsync(ShoppingCart cart)
    {
        var db = _redis.GetDatabase();
        var key = GetRedisKey(cart.UserId);
        var jsonData = JsonConvert.SerializeObject(cart);
        await db.StringSetAsync(key, jsonData);
    }

    public async Task DeleteAsync(string userId)
    {
        var db = _redis.GetDatabase();
        var key = GetRedisKey(userId);
        await db.KeyDeleteAsync(key);
    }

    public async Task<ShoppingCart> GetByUserIdAsync(string userId)
    {
        var db = _redis.GetDatabase();
        var key = GetRedisKey(userId);
        var jsonData = await db.StringGetAsync(key);

        return jsonData.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<ShoppingCart>(jsonData);
    }

    public async Task UpdateAsync(ShoppingCart cart)
    {
        await AddAsync(cart);
    }

    private string GetRedisKey(string userId)
    {
        return $"shoppingcart:{userId}";
    }
}