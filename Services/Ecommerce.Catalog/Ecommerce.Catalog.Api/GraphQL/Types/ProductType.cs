using Ecommerce.Catalog.Domain.Dtos;

namespace Ecommerce.Catalog.Api.GraphQL.Types;

public class ProductType : ObjectType<ProductResponseDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductResponseDto> descriptor)
    {
        descriptor.Field(p => p.Id).Type<IdType>();
        descriptor.Field(p => p.Name).Type<StringType>();
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.Stock).Type<IntType>();
        descriptor.Field(p => p.Discount).Type<DecimalType>();
        descriptor.Field(p => p.Slug).Type<StringType>();
        descriptor.Field(p => p.ImageUrl).Type<StringType>();
        descriptor.Field(p => p.Categories).Type<ListType<CategoryType>>();
    }
}