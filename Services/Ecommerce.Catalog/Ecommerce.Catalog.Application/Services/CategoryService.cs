using AutoMapper;
using Ecommerce.Catalog.Application.Interfaces;
using Ecommerce.Catalog.Domain.Dtos;
using Ecommerce.Catalog.Domain.Entities;
using Ecommerce.Catalog.Domain.UnitOfWork;

namespace Ecommerce.Catalog.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryResponseDto> AddCategory(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        
        await _unitOfWork.CategoryRepository.AddCategory(category);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

        if (category != null)
        {
            try
            {
                _unitOfWork.CategoryRepository.DeleteCategory(category);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                //_logger.LogError($"Error DeleteProduct-DeleteImage id:{id} error:{ex.Message}");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Category {id} not found");
        }
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategories()
    {
        
        var categories = await _unitOfWork.CategoryRepository.GetAllCategories();
        return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
    }

    public async Task<List<CategoryResponseDto>> GetCategoriesByIdsAsync(List<int> categoryIds)
    {
        return _mapper.Map<List<CategoryResponseDto>>(await _unitOfWork.CategoryRepository.GetCategoriesByIdsAsync(categoryIds));
    }

    public async Task<CategoryResponseDto> GetCategoryById(int id)
    {
        return _mapper.Map<CategoryResponseDto>(await _unitOfWork.CategoryRepository.GetCategoryById(id));
    }

    public async Task UpdateCategory(int id, CategoryDto categoryDto)
    {
        var categoty = await _unitOfWork.CategoryRepository.GetCategoryById(id) 
            ?? throw new Exception("Categoty not found");
        
        _mapper.Map(categoryDto, categoty);

        _unitOfWork.CategoryRepository.UpdateCategory(categoty);
        await _unitOfWork.CompleteAsync();
    }
}
