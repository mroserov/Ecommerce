using AutoMapper;
using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Catalog.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ICategoryService categoryService,IMapper mapper, ILogger<ProductController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
    {
        var productImage = new ProductImage
        {
            FileName = productDto.Image?.FileName,
            FileStream = productDto.Image?.OpenReadStream()
        };

        var product = await _productService.AddProduct(productDto, productImage);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto)
    {
        var productImage = new ProductImage
        {
            FileName = productDto.Image?.FileName,
            FileStream = productDto.Image?.OpenReadStream()
        };

        await _productService.UpdateProduct(id, productDto, productImage);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error DeleteProduct id:{id} error:{ex.Message}");
            return Content(ex.Message);
        }
    }
}
