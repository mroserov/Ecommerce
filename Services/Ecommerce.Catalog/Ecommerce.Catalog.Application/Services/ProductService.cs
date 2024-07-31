using AutoMapper;
using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;
using Ecommerce.Catalog.Domain.Services;
using Ecommerce.Catalog.Domain.UnitOfWork;

namespace Ecommerce.Catalog.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllProducts();

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto> GetProductById(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetProductById(id);

        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> AddProduct(ProductDto productDto, ProductImage image)
    {
        var product = _mapper.Map<Product>(productDto);

        product.Categories = await _unitOfWork.CategoryRepository.GetCategoriesByIdsAsync(productDto.CategoryIds);

        if (image.FileStream != null)
        {
            try
            {
                product.ImageUrl = await _fileStorageService.UploadFileAsync(image.FileName, image.FileStream);
            }
            catch (Exception)
            {
                //_logger.Log(LogLevel.Error, ex.InnerException?.Message ?? ex.Message);
            }
        }
        try
        {
            await _unitOfWork.ProductRepository.AddProduct(product);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            await _fileStorageService.DeleteFileAsync(product.ImageUrl);
            throw new Exception($"Error in create product {ex.Message}");
        }
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task UpdateProduct(int id, ProductDto productDto, ProductImage image)
    {
        var product = await _unitOfWork.ProductRepository.GetProductById(id)
            ?? throw new Exception("Product not found");

        _mapper.Map(productDto, product);

        product.Categories = await _unitOfWork.CategoryRepository.GetCategoriesByIdsAsync(productDto.CategoryIds);

        if (image.FileStream != null && !string.IsNullOrEmpty(product.ImageUrl))
        {
            bool isDeleteImage = await _fileStorageService.DeleteFileAsync(product.ImageUrl);
            if (isDeleteImage)
            {
                product.ImageUrl = await _fileStorageService.UploadFileAsync(image.FileName, image.FileStream);
            }
        }
        _unitOfWork.ProductRepository.UpdateProduct(product);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetProductById(id);

        if (product != null)
        {
            try
            {
                _unitOfWork.ProductRepository.DeleteProduct(product);
                await _unitOfWork.CompleteAsync();

                // Delete image
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    await _fileStorageService.DeleteFileAsync(product.ImageUrl);
                }
            }
            catch (Exception)
            {
                //_logger.LogError($"Error DeleteProduct-DeleteImage id:{id} error:{ex.Message}");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Product {id} not found");
        }
    }

    public async Task<PagedResultDto<ProductResponseDto>> GetProductsAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber > 0 ? pageNumber : 1;
        pageSize = pageSize > 0 ? pageSize : 10;

        var products = await _unitOfWork.ProductRepository.GetProducts(searchTerm, pageNumber, pageSize);

        return _mapper.Map<PagedResultDto<ProductResponseDto>>(products);
    }
}
