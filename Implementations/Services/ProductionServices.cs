using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using Dansnom.Enums;

namespace Dansnom.Implementations.Services
{

    public class ProductionServices : IProductionServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IRawMaterialRepository _rawMaterialRepository;
        private readonly IProductionRawMaterialRepository _productionRawMaterialRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IProductServices _productServices;
        public ProductionServices(IProductionRepository productionRepository, IRawMaterialRepository rawMaterialRepository, IProductRepository productRepository, IProductionRawMaterialRepository productionRawMaterialRepository, IProductServices productServices, IAdminRepository adminRepository)
        {
            _productionRepository = productionRepository;
            _rawMaterialRepository = rawMaterialRepository;
            _productRepository = productRepository;
            _productionRawMaterialRepository = productionRawMaterialRepository;
            _productServices = productServices;
            _adminRepository = adminRepository;
        }
        public async Task<BaseResponse> CreateProductionAsync(CreateProductionRequestModel model, List<int> ids, int adminId)
        {
            var admin = await _adminRepository.GetAdminByUserIdAsync(adminId);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Manager not found",
                    Success = false
                };
            }
            var prod = await _productRepository.GetAsync(x => x.Name == model.ProductName && x.IsDeleted == false);
            if (prod == null)
            {
                return new BaseResponse
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            var production = new Production
            {
                AdminId = admin.Id,
                ProductId = prod.Id,
                QuantityRequest = model.QuantityRequest,
                QuantityProduced = model.QuantityProduced,
                QuantityRemaining = model.QuantityProduced,
                ProductionDate = DateTime.Now.ToLongDateString(),
                AdditionalMessage = model.AdditionalMessage,
                ApprovalStatus = ApprovalStatus.Pending
            };
            var prodtions = await _productionRepository.GetAllAsync();
            if (prodtions.Count == 0)
            {
                foreach (var id in ids)
                {
                    var raw = await _rawMaterialRepository.GetAsync(id);

                    if (raw == null)
                    {
                        return new BaseResponse
                        {
                            Message = "Raw Material not found",
                            Success = false
                        };
                    }
                    if (raw.ApprovalStatus == ApprovalStatus.Pending || raw.ApprovalStatus == ApprovalStatus.Rejected)
                    {
                        return new BaseResponse
                        {
                            Message = "Raw Material needs to be aproved",
                            Success = false
                        };
                    }
                    var productionRawMaterial = new ProductionRawMaterial
                    {
                        RawMaterialId = raw.Id,
                        ProductionId = prodtions.Count + 1
                    };
                    production.ProductionRawMaterial.Add(productionRawMaterial);
                }
            }
            else
            {
                foreach (var id in ids)
                {
                    var raw = await _rawMaterialRepository.GetAsync(id);

                    if (raw == null)
                    {
                        return new BaseResponse
                        {
                            Message = "Raw Material not found",
                            Success = false
                        };
                    }
                    if (raw.ApprovalStatus == ApprovalStatus.Pending || raw.ApprovalStatus == ApprovalStatus.Rejected)
                    {
                        return new BaseResponse
                        {
                            Message = "Raw Material needs to be aproved",
                            Success = false
                        };
                    }
                    var productionRawMaterial = new ProductionRawMaterial
                    {
                        RawMaterialId = raw.Id,
                        ProductionId = prodtions[prodtions.Count - 1].Id + 1
                    };
                    production.ProductionRawMaterial.Add(productionRawMaterial);
                }
            }
            var prodtion = await _productionRepository.CreateAsync(production);
            await _productionRepository.UpdateAsync(production);
            return new BaseResponse
            {
                Message = "Production Created Successfully",
                Success = true
            };
        }
        public async Task<ProductionsResponseModel> GetAllAprovedProductionsByYearAsync(int year)
        {
            var productions = await _productionRawMaterialRepository.GetAllAprovedProductionsByYearAsync(year);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Aproved production found for {year}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllRejectedProductionsByYearAsync(int year)
        {
            var productions = await _productionRawMaterialRepository.GetAllRejectedProductionsByYearAsync(year);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Rejected production found for {year}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Rejected Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllAprovedProductionsByMonthAsync(int month)
        {
            var productions = await _productionRawMaterialRepository.GetAllAprovedProductionsByMonthAsync(month);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Aproved production found for {month}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Approved Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllRejectedProductionsByMonthAsync(int month)
        {
            var productions = await _productionRawMaterialRepository.GetAllRejectedProductionsByMonthAsync(month);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Rejected production found for {month}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Rejected Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllPendingProductionsAsync()
        {
            var productions = await _productionRawMaterialRepository.GetAllPendingProductionsAsync();
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = "no Pending production found for",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Pending Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetProductionsByDateAsync(string date)
        {
            var productions = await _productionRawMaterialRepository.GetProductionsByDate(date);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no production found for {date}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetProductionsByProductIdAsync(int id)
        {
            var productions = await _productionRawMaterialRepository.GetProductionsByProductId(id);
            var prod = await _productRepository.GetAsync(id);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no production found for {prod.Name}",
                    Success = false
                };
            }
            return new ProductionsResponseModel
            {
                Message = "Productins found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    ApprovalStatus = x.Production.ApprovalStatus,
                    ProductionDate = x.Production.ProductionDate,
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,
                    AdditionalMessage = x.Production.AdditionalMessage,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Production.ProductId,
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl,
                        isAvailable = x.Production.Product.isAvailable
                    },
                    RawMaterialDto = new RawMaterialDto
                    {
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        Name = x.RawMaterial.Name,
                        AdditionalMessage = x.RawMaterial.AdditionalMessage,
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByYearAsync(int year)
        {

            var product = await _productRepository.GetAllAsync();
            if (product.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<Production> productions = new List<Production>();
            foreach (var item in product)
            {
                var prodtion = await _productionRawMaterialRepository.GetAllApprovedYearlyProduction(year, item.Id);
                var production = new Production
                {
                    QuantityProduced = prodtion.Sum(x => x.Production.QuantityProduced),
                    Product = new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        Price = item.Price,
                        isAvailable = item.isAvailable
                    },
                };
                productions.Add(production);
            }
            return new ProductionsResponseModel
            {
                Message = "Production for each product successfully found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    QuantityProduced = x.QuantityProduced,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl,
                        Price = x.Product.Price,
                        isAvailable = x.Product.isAvailable
                    }
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductByMonthAsync(int month)
        {

            var product = await _productRepository.GetAllAsync();
            if (product.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<Production> productions = new List<Production>();
            foreach (var item in product)
            {
                var prodtion = await _productionRawMaterialRepository.GetAllApprovedMonthlyProduction(month, item.Id);
                var production = new Production
                {
                    QuantityProduced = prodtion.Sum(x => x.Production.QuantityProduced),
                    Product = new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        Price = item.Price,
                        isAvailable = item.isAvailable
                    },
                };
                productions.Add(production);
            }
            return new ProductionsResponseModel
            {
                Message = "Production for each product successfully found",
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    QuantityProduced = x.QuantityProduced,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl,
                        Price = x.Product.Price,
                        isAvailable = x.Product.isAvailable
                    }
                }).ToList()
            };
        }
        public async Task<BaseResponse> UpdateProductionAsync(int id, UpdateProductionRequestModel model)
        {
            var production = await _productionRepository.GetAsync(x => x.Id == id);
            var product = await _productRepository.GetAsync(x => x.Name == model.ProductName);
            if (production == null)
            {
                return new BaseResponse
                {
                    Message = "Production not found",
                    Success = false
                };
            }
            if (production.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "This request has been approved and can't be updated",
                    Success = false
                };
            }
            production.QuantityProduced = model.QuantityProduced;
            production.ProductId = product.Id;
            production.QuantityRemaining = model.QuantityProduced - (production.QuantityProduced - production.QuantityRemaining);
            production.ApprovalStatus = ApprovalStatus.Pending;
            await _productionRepository.UpdateAsync(production);
            return new BaseResponse
            {
                Message = "Production updated successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> AproveProduction(int id)
        {
            var production = await _productionRepository.GetAsync(id);
            var productionRawMaterial = await _productionRawMaterialRepository.GetProductionsById(id);
            if (production == null || productionRawMaterial.Count == 0)
            {
                return new BaseResponse
                {
                    Message = "Production not found",
                    Success = false
                };
            }
            foreach (var rawMaterial in productionRawMaterial)
            {
                var material = await _rawMaterialRepository.GetAsync(x => x.Id == rawMaterial.RawMaterial.Id && x.IsDeleted == false);
                if (material.QuantiityRemaining < production.QuantityRequest)
                {
                    return new BaseResponse
                    {
                        Message = "Request can't be approved because quantity of raw material is not up to quantity requested",
                        Success = false
                    };
                }
                material.QuantiityRemaining -= production.QuantityRequest;
                await _rawMaterialRepository.UpdateAsync(material);
            }
            production.ApprovalStatus = ApprovalStatus.Approved;
            await _productionRepository.UpdateAsync(production);
            await _productServices.UpdateProductsAvailability();
            return new BaseResponse
            {
                Message = "Production Aproved successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> RejectProduction(int id)
        {
            var production = await _productionRepository.GetAsync(id);
            if (production == null)
            {
                return new BaseResponse
                {
                    Message = "Production not found",
                    Success = false
                };
            }
            production.ApprovalStatus = ApprovalStatus.Rejected;
            await _productionRepository.UpdateAsync(production);
            await _productServices.UpdateProductsAvailability();
            return new BaseResponse
            {
                Message = "Production rejected successfully",
                Success = true
            };
        }
    }
}