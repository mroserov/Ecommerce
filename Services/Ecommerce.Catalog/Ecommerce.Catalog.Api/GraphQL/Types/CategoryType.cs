using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;

namespace Ecommerce.Catalog.Api.GraphQL.Types;

public class CategoryType : ObjectType<CategoryResponseDto>
{
    protected override void Configure(IObjectTypeDescriptor<CategoryResponseDto> descriptor)
    {
        descriptor.Field(c => c.Id).Type<IdType>();
        descriptor.Field(c => c.Name).Type<StringType>();
    }
}