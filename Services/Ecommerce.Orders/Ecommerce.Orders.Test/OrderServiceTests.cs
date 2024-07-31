namespace Ecommerce.Orders.Test;

using AutoMapper;
using Ecommerce.Orders.Application.Interfaces;
using Ecommerce.Orders.Application.Services;
using Ecommerce.Orders.Domain.UnitOfWork;
using Ecommerce.Orders.Domain.Dtos;
using Ecommerce.Orders.Domain.Entities;
using Moq;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IOrderService _orderService;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _orderService = new OrderService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldReturnOrderDto()
    {
        // Arrange
        var orderDto = new OrderDto { Address = "1", Email = "1", OrderItems = new List<OrderItemDto>()};
        var order = new Order { Id = 1, UserId = Guid.NewGuid(), OrderItems = new List<OrderItem>() };

        _mapperMock.Setup(m => m.Map<Order>(It.IsAny<OrderDto>())).Returns(order);
        _mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(orderDto);

        _unitOfWorkMock.Setup(u => u.OrderRepository.Add(It.IsAny<Order>()));
        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        var result = await _orderService.CreateOrderAsync(orderDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderDto.Email, result.Email);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrderDto()
    {
        // Arrange
        var orderDto = new OrderDto { Address = "1", Email = "1", OrderItems = new List<OrderItemDto>() };
        var order = new Order { Id = 1, UserId = Guid.NewGuid(), OrderItems = new List<OrderItem>()};

        _mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(orderDto);
        _unitOfWorkMock.Setup(u => u.OrderRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(order);

        // Act
        var result = await _orderService.GetOrderByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderDto.Email, result.Email);
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldReturnListOfOrderDto()
    {
        // Arrange
        var orderDtos = new List<OrderDto>
        {
            new OrderDto { Address  = "1", Email = "1", OrderItems = new List<OrderItemDto>() },
            new OrderDto { Address  = "2", Email = "2", OrderItems = new List<OrderItemDto>() }
        };
        var orders = new List<Order>
        {
            new Order { Id = 1,  Email = "1", OrderItems = new List<OrderItem>() },
            new Order { Id = 2,  Email = "2", OrderItems = new List<OrderItem>() }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<OrderDto>>(It.IsAny<IEnumerable<Order>>())).Returns(orderDtos);
        _unitOfWorkMock.Setup(u => u.OrderRepository.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _orderService.GetAllOrdersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldUpdateOrder()
    {
        // Arrange
        var orderDto = new OrderDto { Address = "1", Email = "1", OrderItems = new List<OrderItemDto>() };
        var order = new Order { Address = "1", Email = "1", OrderItems = new List<OrderItem>() };

        _mapperMock.Setup(m => m.Map<Order>(It.IsAny<OrderDto>())).Returns(order);
        _unitOfWorkMock.Setup(u => u.OrderRepository.Update(It.IsAny<Order>()));
        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _orderService.UpdateOrderAsync(orderDto);

        // Assert
        _unitOfWorkMock.Verify(u => u.OrderRepository.Update(It.IsAny<Order>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldRemoveOrder()
    {
        // Arrange
        var order = new Order { Address = "1", Email = "1", OrderItems = new List<OrderItem>() };

        _unitOfWorkMock.Setup(u => u.OrderRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(order);
        _unitOfWorkMock.Setup(u => u.OrderRepository.Remove(It.IsAny<Order>()));
        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        // Act
        await _orderService.DeleteOrderAsync(1);

        // Assert
        _unitOfWorkMock.Verify(u => u.OrderRepository.Remove(It.IsAny<Order>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}