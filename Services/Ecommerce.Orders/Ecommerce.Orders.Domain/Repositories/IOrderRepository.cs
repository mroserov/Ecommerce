using Ecommerce.Orders.Domain.Entities;

namespace Ecommerce.Orders.Domain.Repositories;
public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    void Update(Order order);
    void Remove(Order order);
}
