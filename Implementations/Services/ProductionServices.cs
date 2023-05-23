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
                            Message = $"{raw.Name} Raw Material needs to be aproved",
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
        public async Task<ProductionsResponseModel> GetAllAprovedProductionsAsync()
        {
            var productions = await _productionRepository.GetAllApprovedProduction();
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Aproved production found for",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    ApprovalStatus = prodtion.ApprovalStatus.ToString(),
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                        EnumApprovalStatus = x.RawMaterial.ApprovalStatus
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest,
                    ApprovalStatus = x.ApprovalStatus
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllRejectedProductionsAsync()
        {
            var productions = await _productionRepository.GetAllRejectedProduction();
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Rejected production",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllAprovedProductionsByMonthAsync(int year, int month)
        {
            var productions = await _productionRepository.GetAllAprovedProductionsByMonthAsync(year, month);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no Aproved production found for {month}",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllPendingProductionsAsync()
        {
            var productions = await _productionRepository.GetAllPendingProductionsAsync();
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = "no Pending production found for",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetProductionsByDateAsync(string date)
        {
            var productions = await _productionRepository.GetProductionsByDate(date);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no production found for {date}",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production for" + date + "retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetProductionsByProductIdAsync(int id)
        {
            var prod = await _productRepository.GetAsync(id);
            if (prod == null)
            {
                return new ProductionsResponseModel
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            var productions = await _productionRepository.GetProductionsByProductId(prod.Id);
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = $"no production found for {prod.Name}",
                    Success = false
                };
            }
            var ProductionDto = new List<ProductionDto>();
            foreach (var prodtion in productions)
            {
                var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                var productionDto = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = prodtion.Product.Name,
                        Price = prodtion.Product.Price,
                        ImageUrl = prodtion.Product.ImageUrl,
                        isAvailable = prodtion.Product.isAvailable,
                    },
                    ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                    QuantityRemaining = prodtion.QuantityRemaining,
                    QuantityProduced = prodtion.QuantityProduced,
                    QuantityRequest = prodtion.QuantityRequest,
                    ApprovalStatus = prodtion.ApprovalStatus.ToString(),
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Id = x.RawMaterial.Id,
                        Name = x.RawMaterial.Name,
                        Cost = x.RawMaterial.Cost,
                        QuantiityBought = x.RawMaterial.QuantiityBought,
                        QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                        StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                    }).ToList()
                };
                ProductionDto.Add(productionDto);
            }
            return new ProductionsResponseModel
            {
                Message = "Production for" + prod.Name + "retrieved successfully",
                Success = true,
                Data = ProductionDto.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
                }).ToList()
            };
        }
        public async Task<ProductionsResponseModel> GetAllApprovedProductionsOnEachProductAsync()
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
            List<ProductionDto> productionDtos = new List<ProductionDto>();
            foreach (var item in product)
            {
                var productions = await _productionRepository.GetProductionsByProductId(item.Id);
                foreach (var prodtion in productions)
                {
                    var production = await _productionRawMaterialRepository.GetProductionsById(prodtion.Id);
                    var productionDto = new ProductionDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = prodtion.Product.Name,
                            Price = prodtion.Product.Price,
                            ImageUrl = prodtion.Product.ImageUrl,
                            isAvailable = prodtion.Product.isAvailable,
                        },
                        ProductionDate = prodtion.LastModifiedOn.ToLongDateString(),
                        QuantityRemaining = prodtion.QuantityRemaining,
                        QuantityProduced = prodtion.QuantityProduced,
                        QuantityRequest = prodtion.QuantityRequest,
                        RawMaterialDto = production.Select(x => new RawMaterialDto
                        {
                            Id = x.RawMaterial.Id,
                            Name = x.RawMaterial.Name,
                            Cost = x.RawMaterial.Cost,
                            QuantiityBought = x.RawMaterial.QuantiityBought,
                            QuantiityRemaining = x.RawMaterial.QuantiityRemaining,
                            StringApprovalStatus = x.RawMaterial.ApprovalStatus.ToString(),
                        }).ToList()
                    };
                    productionDtos.Add(productionDto);
                }
            }
            return new ProductionsResponseModel
            {
                Message = "Production for each product successfully found",
                Success = true,
                Data = productionDtos.Select(x => new ProductionDto
                {
                    ProductDto = x.ProductDto,
                    RawMaterialDto = x.RawMaterialDto,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest
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
        public async Task<ProductionsResponseModel> GetAllProductionAsync()
        {
            var productions = await _productionRepository.GetAllProductionsAsync();
            if (productions.Count == 0)
            {
                return new ProductionsResponseModel
                {
                    Message = "No Production request yet",
                    Success = false
                };
            }
            foreach (var item in productions)
            {

                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new ProductionsResponseModel
            {
                Success = true,
                Data = productions.Select(x => new ProductionDto
                {
                    Admin = new UserDto
                    {
                        UserName = x.Admin.User.Username,
                        Image = x.Admin.User.ProfileImage
                    },
                    ProductionId = x.Id,
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ApprovalStatus = x.ApprovalStatus.ToString(),
                    PostedTime = x.RequestTime,
                    ProductionDate = x.ProductionDate,
                    QuantityProduced = x.QuantityProduced,
                    QuantityRemaining = x.QuantityRemaining,
                    QuantityRequest = x.QuantityRequest,
                    ProductDto = new ProductDto
                    {
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl
                    }
                }).ToList()
            };
        }
        public async Task<BaseResponse> UpdateProductionAsync(int id, UpdateProductionRequestModel model, List<int> ids)
        {
            var production = await _productionRawMaterialRepository.GetProductionsById(id);
            var product = await _productRepository.GetAsync(x => x.Name == model.ProductName);
            if (production == null)
            {
                return new BaseResponse
                {
                    Message = "Production not found",
                    Success = false
                };
            }
            if (production[0].Production.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "This request has been approved and can't be updated",
                    Success = false
                };
            }
            production[0].Production.QuantityProduced = model.QuantityProduced;
            production[0].Production.QuantityRequest = model.QuantityRequest;
            production[0].Production.AdditionalMessage = model.AdditionalMessage ?? production[0].Production.AdditionalMessage;
            production[0].Production.ProductId = product.Id;
            production[0].Production.QuantityRemaining = model.QuantityProduced - (production[0].Production.QuantityProduced - production[0].Production.QuantityRemaining);
            production[0].Production.ApprovalStatus = ApprovalStatus.Pending;
            for (int i = 0; i < production.Count; i++)
            {
                if (i > ids.Count - 1)
                {
                    // production.RemoveRange(i, production.Count - i);
                    for (int j = i; j < production.Count; j++)
                    {
                        production[i].IsDeleted = true;
                    }
                    break;
                }
                production[i].RawMaterialId = ids[i];
            }
            foreach (var item in production)
            {
                await _productionRawMaterialRepository.UpdateAsync(item);
            }
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
            production.LastModifiedOn = DateTime.Now;
            await _productionRepository.UpdateAsync(production);
            await _productServices.UpdateProductsAvailability();
            return new BaseResponse
            {
                Message = "Production Aproved successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> RejectProduction(int id, RejectRequestRequestModel model)
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
            production.AdditionalMessage = model.Message;
            await _productionRepository.UpdateAsync(production);
            await _productServices.UpdateProductsAvailability();
            return new BaseResponse
            {
                Message = "Production rejected successfully",
                Success = true
            };
        }
        public async Task<ProductionResponseModel> GetProductionById(int id)
        {
            var production = await _productionRawMaterialRepository.GetProductionsById(id);
            if (production.Count == 0)
            {
                return new ProductionResponseModel
                {
                    Message = "Production not found",
                    Success = false
                };
            }
            return new ProductionResponseModel
            {
                Message = "Production found successfully",
                Success = true,
                Data = new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = production[0].Production.Product.Name
                    },
                    QuantityRequest = production[0].Production.QuantityRequest,
                    QuantityProduced = production[0].Production.QuantityProduced,
                    AdditionalMessage = production[0].Production.AdditionalMessage,
                    ApprovalStatus = production[0].Production.ApprovalStatus.ToString(),
                    RawMaterialDto = production.Select(x => new RawMaterialDto
                    {
                        Name = x.RawMaterial.Name
                    }).ToList()
                }
            };
        }
        public async Task<BaseResponse> DeleteAsync(int id)
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
            if (production.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "Production has been approved already",
                    Success = false
                };
            }
            production.IsDeleted = true;
            await _productionRepository.UpdateAsync(production);
            return new BaseResponse
            {
                Message = "Production Successfully Deleted",
                Success = true
            };
        }
    }
}