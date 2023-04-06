using System;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<BaseResponse> CreateCategory(CreateCategoryRequestModel model)
    {
        if (model.Name == "")
        {
            return new BaseResponse
            {
                Message = "Category form is empty",
                Success = false
            };
        }
        var exist = await _categoryRepository.ExistsAsync(x => x.Name == model.Name);
        if (exist)
        {
            return new BaseResponse
            {
                Message = "Category Already EXist",
                Success = false
            };
        }
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };
        await _categoryRepository.CreateAsync(category);
        return new BaseResponse
        {
            Message = "Category Successfully Created",
            Success = true
        };
    }
    public async Task<CategoriesResponseModel> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        if (categories.Count == 0)
        {
            return new CategoriesResponseModel
            {
                Message = "No categories yet",
                Success = false
            };
        }
        return new CategoriesResponseModel
        {
            Message = "Categories found",
            Success = true,
            Data = categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList()
        };
    }
    public async Task<CategoryResponseModel> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(id);
        if (category == null)
        {
            return new CategoryResponseModel
            {
                Message = "Category not found",
                Success = false
            };
        }
        return new CategoryResponseModel
        {
            Data = new CategoryDto
            {
                Name = category.Name,
                Description = category.Description
            }
        };
    }
}