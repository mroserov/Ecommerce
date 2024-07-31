using AutoMapper;
using Ecommerce.Orders.Application.Interfaces;
using Ecommerce.Orders.Domain.Dtos;
using Ecommerce.Orders.Domain.Entities;
using Ecommerce.Orders.Domain.UnitOfWork;

namespace Ecommerce.Orders.Application.Services;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(OrderDto createOrderDto)
    {
        var order = _mapper.Map<Order>(createOrderDto);
        _unitOfWork.OrderRepository.Add(order);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.OrderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderAsync(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        _unitOfWork.OrderRepository.Update(order);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        _unitOfWork.OrderRepository.Remove(order);
        await _unitOfWork.CommitAsync();
    }
}
