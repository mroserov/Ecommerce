using AutoMapper;
using Ecommerce.Basket.Domain.Entities;

namespace Ecommerce.Basket.Api.Profiles;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ShoppingCartItem, ShoppingCartItem>();
        CreateMap<ShoppingCart, ShoppingCart>();
    }
}