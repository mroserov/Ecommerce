using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.Catalog.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<CategoryResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategories()
        {
            var products = await _categoryService.GetAllCategories();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var product = await _categoryService.GetCategoryById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto categoryDto)
        {
            var category = await _categoryService.AddCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDto categoryDto)
        {
            await _categoryService.UpdateCategory(id, categoryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error DeleteCategory id:{id} error:{ex.Message}");
                return Content(ex.Message);
            }
        }
    }
}
