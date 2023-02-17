using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using Microsoft.AspNetCore.Hosting;

namespace Dansnom.Implementations.Services
{

    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductServices(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, IProductionRepository productionRepository)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _productionRepository = productionRepository;
        }
        public async Task<BaseResponse> CreateProduct(CreateProductRequestModel model)
        {
            var exist = await _productRepository.ExistsAsync(p => p.Name == model.Name.ToLower() && p.IsDeleted == false);
            if (exist)
            {
                return new BaseResponse
                {
                    Message = "Product Already Exist",
                    Success = false
                };
            }
            var imageName = "";
            if (model.ImageUrl != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ImageUrl.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }
            var product = new Product
            {
                Name = model.Name.ToLower(),
                Price = model.Price,
                ImageUrl = imageName,
                isAvailable = false,
                Description = model.Description
            };
            await _productRepository.CreateAsync(product);
            return new BaseResponse
            {
                Message = "Product Successfully created",
                Success = true
            };
        }
        public async Task<ProductsResponseModel> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            if (products == null)
            {
                return new ProductsResponseModel
                {
                    Message = "Products not found",
                    Success = false
                };
            }
            foreach (var prod in products)
            {
                var production = await _productionRepository.GetProductionsByProductId(prod.Id);
            }
            return new ProductsResponseModel
            {
                Message = "Products found",
                Success = true,
                Data = products.Select(x => new ProductDto
                {
                    ProductId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    QuantityRemaining =  _productionRepository.GetQuantityRemainingByProductId(x.Id),
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    isAvailable = x.isAvailable
                }).ToList()
            };
        }
        public async Task<ProductResponseModel> GetProductById(int id)
        {
            var prod = await _productRepository.GetAsync(id);
            if (prod == null)
            {
                return new ProductResponseModel
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            return new ProductResponseModel
            {
                Message = "Product found",
                Success = true,
                Data = new ProductDto
                {
                    ProductId = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    ImageUrl = prod.ImageUrl,
                    isAvailable = prod.isAvailable
                }
            };
        }
        public async Task<BaseResponse> UpdateProduct(UpdateProductRequestModel model, int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return new ProductResponseModel
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            var imageName = "";
            if (model.ImageUrl != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ImageUrl.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }
            product.Name = model.Name.ToLower() ?? product.Name;
            product.Price = model.Price;
            product.ImageUrl = imageName ?? product.ImageUrl;
            product.isAvailable = model.isAvailable;

            if (model.isAvailable == false)
            {
                var production = await _productionRepository.GetProductionsByProductId(id);
                foreach (var prod in production)
                {
                    prod.QuantityRemaining = 0.0m;
                    await _productionRepository.UpdateAsync(prod);
                }
            }
            await _productRepository.UpdateAsync(product);

            return new BaseResponse
            {
                Message = "Product Updeted Successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(c => c.Id == id && c.IsDeleted == false);
            if (product == null)
            {
                return new BaseResponse
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            product.IsDeleted = true;
            await _productRepository.UpdateAsync(product);
            return new BaseResponse
            {
                Message = "Product Successfully deleted",
                Success = true
            };
        }
        public async Task<ProductsResponseModel> GetProductsReadyForDelivery()       
        {
            var update = await UpdateProductsAvailability();
            if (update.Success == false)
            {
                return new ProductsResponseModel
                {
                    Message = update.Message,
                    Success = false
                };
            }
            var products = await _productRepository.GetAllAvailableProductAsync();
            if(products.Count == 0)
            {
                return new ProductsResponseModel
                {
                    Message = "No product is available at the moment",
                    Success = false
                };
            }
             return new ProductsResponseModel
            {
                Message = "This are the available products",
                Success = true,
                Data = products.Select(x => new ProductDto
                {
                    ProductId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    QuantityRemaining =  _productionRepository.GetQuantityRemainingByProductId(x.Id),
                    isAvailable = x.isAvailable
                }).ToList()
            };
        }
        public async Task<BaseResponse> UpdateProductsAvailability()
        {
            var products = await _productRepository.GetAllAsync();
            if(products.Count == 0)
            {
                return new BaseResponse
                {
                    Message = "No product found",
                    Success = false
                };
            }
            foreach (var product in products)
            {
                var quantity = _productionRepository.GetQuantityRemainingByProductId(product.Id);
                if(quantity == 0.0m)
                {
                    product.isAvailable = false;
                }
                else if (quantity > 0.0m)
                {
                    product.isAvailable = true;
                }
                   await _productRepository.UpdateAsync(product);
            }
            return new BaseResponse
            {
                Message = "Successfuly updated",
                Success = true
            };
        }
    }
}