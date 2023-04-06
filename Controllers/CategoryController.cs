using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dansnom.Interface.Services;
using Dansnom.Dtos.RequestModel;
using Project.Controllers.Base;
using Microsoft.AspNetCore.Http;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : MyControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("AddNewCategory")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody]CreateCategoryRequestModel model)
        {
            var category = await _categoryService.CreateCategory(model);
            if (category.Success == true)
            {
                return Ok(category);
            }
            return BadRequest(category);
        }
        [HttpGet("GetProductCategoties")]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            if (categories.Success == true)
            {
                return Ok(categories);
            }
            return BadRequest(categories);
        }
        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category.Success == true)
            {
                return Ok(category);
            }
            return BadRequest(category);
        }
    }
}