using AutoMapper;
using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;

namespace Ecommerce.Catalog.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

        CreateMap<Category, CategoryResponseDto>();

        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

        CreateMap<CategoryDto, Category>();

        CreateMap<PagedResultDto<Product>, PagedResultDto<ProductResponseDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}