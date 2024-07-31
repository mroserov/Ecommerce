using Ecommerce.Basket.Application.Services;
using Ecommerce.Basket.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Basket.Api.Controllers;
[Route("api/[Controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetShoppingCart(string userId)
    {
        var cart = await _shoppingCartService.GetShoppingCartAsync(userId);
        if (cart == null)
        {
            return NotFound();
        }
        return Ok(cart);
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddItemToShoppingCart(string userId, [FromBody] ShoppingCartItem item)
    {
        await _shoppingCartService.AddItemToShoppingCartAsync(userId, item);
        return Ok();
    }

    [HttpPut("{userId}/items")]
    public async Task<IActionResult> UpdateItemInShoppingCart(string userId, [FromBody] ShoppingCartItem item)
    {
        await _shoppingCartService.UpdateItemInShoppingCartAsync(userId, item);
        return Ok();
    }

    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItemFromShoppingCart(string userId, string productId)
    {
        await _shoppingCartService.RemoveItemFromShoppingCartAsync(userId, productId);
        return Ok();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearShoppingCart(string userId)
    {
        await _shoppingCartService.ClearShoppingCartAsync(userId);
        return Ok();
    }
}
