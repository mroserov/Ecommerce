using Ecommerce.Catalog.Application.Interfaces;

namespace Ecommerce.Catalog.Api.GraphQL.Mutations;

[ExtendObjectType(Name = "Mutation")]
public class ProductMutations
{
    //public async Task<ProductResponseDto> AddProductAsync([Service] IProductService productService, ProductDto productDto, IFile file)
    //{
    //    return await productService.AddProduct(productDto, null);
    //}

    //public async Task<ProductResponseDto> UpdateProductAsync([Service] IProductService productService, int id, ProductDto productDto, IFile file)
    //{
    //    await productService.UpdateProduct(id, productDto, null);
    //    return await productService.GetProductById(id);
    //}

    public async Task<bool> DeleteProductAsync([Service] IProductService productService, int id)
    {
        await productService.DeleteProduct(id);
        return true;
    }
}
