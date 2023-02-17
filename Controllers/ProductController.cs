using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dansnom.Interface.Services;
using Dansnom.Dtos.RequestModel;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateProductRequestModel model)
        {
            var product = await _productServices.CreateProduct(model);
            if (product.Success == false)
            {
                return BadRequest(product);
            }
            return Ok(product);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var product = await _productServices.GetProductById(id);
            if (product.Success == false)
            {
                return BadRequest(product);
            }
            return Ok(product);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productServices.GetAllProducts();
            if (products.Success == false)
            {
                return BadRequest(products);
            }
            return Ok(products);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var product = await _productServices.DeleteProductAsync(id);
            if (product.Success == false)
            {
                return BadRequest(product);
            }
            return Ok(product);
        }
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateProductRequestModel model,[FromRoute] int id)
        {
            var product = await _productServices.UpdateProduct(model,id);
            if (product.Success == false)
            {
                return BadRequest(product);
            }
            return Ok(product);
        }
        [HttpGet("GetProductsReadyForDelivery")]
        public async Task<IActionResult> GetProductsReadyForDelivery()
        {
            var products = await _productServices.GetProductsReadyForDelivery();
            if (products.Success == false)
            {
                return BadRequest(products);
            }
            return Ok(products);
        }
    }
}