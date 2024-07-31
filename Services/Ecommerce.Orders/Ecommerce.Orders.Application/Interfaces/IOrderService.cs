using Ecommerce.Orders.Domain.Dtos;

namespace Ecommerce.Orders.Application.Interfaces;
public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task UpdateOrderAsync(OrderDto orderDto);
    Task DeleteOrderAsync(int id);
}
