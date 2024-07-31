using AutoMapper;
using Ecommerce.Basket.Application.Services;
using Ecommerce.Basket.Domain.Entities;
using Ecommerce.Basket.Domain.Interfaces;
using Moq;

namespace Ecommerce.Basket.Test;

public class ShoppingCartServiceTests
{
    private readonly Mock<IShoppingCartRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ShoppingCartService _service;

    public ShoppingCartServiceTests()
    {
        _repositoryMock = new Mock<IShoppingCartRepository>();
        var config = new MapperConfiguration(cfg =>
        {
            // Configure AutoMapper if needed
        });
        _mapper = config.CreateMapper();
        _service = new ShoppingCartService(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetShoppingCartAsync_ShouldReturnCart_WhenCartExists()
    {
        // Arrange
        var userId = "test-user";
        var expectedCart = new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem>() };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(expectedCart);

        // Act
        var result = await _service.GetShoppingCartAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCart, result);
    }

    [Fact]
    public async Task AddItemToShoppingCartAsync_ShouldAddNewItem_WhenItemDoesNotExist()
    {
        // Arrange
        var userId = "test-user";
        var newItem = new ShoppingCartItem { Id = "item1", Quantity = 1, Price = 10 };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync((ShoppingCart)null);

        // Act
        await _service.AddItemToShoppingCartAsync(userId, newItem);

        // Assert
        _repositoryMock.Verify(repo => repo.AddAsync(It.Is<ShoppingCart>(c => c.Items.Contains(newItem))), Times.Once);
    }

    [Fact]
    public async Task AddItemToShoppingCartAsync_ShouldUpdateExistingItem_WhenItemExists()
    {
        // Arrange
        var userId = "test-user";
        var existingItem = new ShoppingCartItem { Id = "item1", Quantity = 1, Price = 10 };
        var cart = new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem> { existingItem } };
        var newItem = new ShoppingCartItem { Id = "item1", Quantity = 2, Price = 15 };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(cart);

        // Act
        await _service.AddItemToShoppingCartAsync(userId, newItem);

        // Assert
        Assert.Equal(3, existingItem.Quantity);
        Assert.Equal(15, existingItem.Price);
        _repositoryMock.Verify(repo => repo.AddAsync(cart), Times.Once);
    }

    [Fact]
    public async Task UpdateItemInShoppingCartAsync_ShouldUpdateItem_WhenItemExists()
    {
        // Arrange
        var userId = "test-user";
        var existingItem = new ShoppingCartItem { Id = "item1", Quantity = 1, Price = 10 };
        var cart = new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem> { existingItem } };
        var updatedItem = new ShoppingCartItem { Id = "item1", Quantity = 3, Price = 20 };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(cart);

        // Act
        await _service.UpdateItemInShoppingCartAsync(userId, updatedItem);

        // Assert
        Assert.Equal(3, existingItem.Quantity);
        Assert.Equal(20, existingItem.Price);
        _repositoryMock.Verify(repo => repo.UpdateAsync(cart), Times.Once);
    }

    [Fact]
    public async Task RemoveItemFromShoppingCartAsync_ShouldRemoveItem_WhenItemExists()
    {
        // Arrange
        var userId = "test-user";
        var item = new ShoppingCartItem { Id = "item1", Quantity = 1, Price = 10 };
        var cart = new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem> { item } };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(cart);

        // Act
        await _service.RemoveItemFromShoppingCartAsync(userId, "item1");

        // Assert
        Assert.Empty(cart.Items);
        _repositoryMock.Verify(repo => repo.UpdateAsync(cart), Times.Once);
    }

    [Fact]
    public async Task ClearShoppingCartAsync_ShouldClearAllItems_WhenCartExists()
    {
        // Arrange
        var userId = "test-user";
        var cart = new ShoppingCart { Id = Guid.NewGuid(), UserId = userId, Items = new List<ShoppingCartItem> { new ShoppingCartItem { Id = "item1", Quantity = 1, Price = 10 } } };
        _repositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(cart);

        // Act
        await _service.ClearShoppingCartAsync(userId);

        // Assert
        Assert.Empty(cart.Items);
        _repositoryMock.Verify(repo => repo.UpdateAsync(cart), Times.Once);
    }
}