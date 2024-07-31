using AutoMapper;
using Ecommerce.Orders.Domain.Dtos;
using Ecommerce.Orders.Domain.Entities;

namespace Ecommerce.Orders.Api.MappingProfile;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderDto, Order>();
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
    }
}
