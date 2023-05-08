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

namespace Dansnom.Implementations.Services
{
    public class SalesServices : ISalesServices
    {
        private readonly ISalesRepository _salesRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWalletServices _walletServices;
        private readonly IProductOrdersRepository _productOrdersRepository;
        private readonly IRawMaterialRepository _ramMaterialRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IOrderRepository _orderRepository;
        public SalesServices(ISalesRepository salesRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IWalletServices walletServices, IProductOrdersRepository productOrdersRepository, IRawMaterialRepository expensesRepository, IProductionRepository productionRepository, IOrderRepository orderRepository)
        {
            _salesRepository = salesRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _walletServices = walletServices;
            _productOrdersRepository = productOrdersRepository;
            _ramMaterialRepository = expensesRepository;
            _productionRepository = productionRepository;
            _orderRepository = orderRepository;
        }
        public async Task<BaseResponse> CreateSales(int id)
        {
            var order = await _productOrdersRepository.GetOrdersByIdAsync(id);
            if (order == null)
            {
                return new BaseResponse
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            decimal totalAmount = 0;
            foreach (var item in order)
            {
                totalAmount += item.Product.Price * item.QuantityBought;
            }
            var sales = new Sales
            {
                OrderId = order[0].Order.Id,
                // ProductId = productOrders.ProductId,
                AmountPaid = totalAmount
            };
            var sale = await _salesRepository.CreateAsync(sales);
            var wallet = await _walletServices.FundWallet(totalAmount);

            return new BaseResponse
            {
                Message = "Sale created successfully",
                Success = true
            };
        }

        public async Task<SalesResponseModel> GetSalesByCustomerNameAsync(string name)
        {
            var customer = await _customerRepository.GetAsync(x => x.User.Username == name);
            if (customer == null)
            {
                return new SalesResponseModel
                {
                    Message = $"Customer with {name} is not registered on this app",
                    Success = false
                };
            }
            var sales = await _salesRepository.GetSaleByCustomerIdAsync(customer.Id);
            if (sales.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = "no sales found for this customer",
                    Success = false
                };
            }
            List<ProductOrdersDto> productOrdersDtos = new List<ProductOrdersDto>();
            foreach (var item in sales)
            {
                var order = await _productOrdersRepository.GetOrdersByIdAsync(item.OrderId);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = order[0].Order.Customer.FullName,
                        Username = order[0].Order.Customer.User.Username,
                        PhoneNumber = order[0].Order.Customer.PhoneNumber,
                        Email = order[0].Order.Customer.User.Email,
                        ImageUrl = order[0].Order.Customer.User.ProfileImage
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                        AdditionalDetails = order[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = order.Select(x => new OrderDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            ImageUrl = x.Product.ImageUrl,
                            isAvailable = x.Product.isAvailable,
                            Description = x.Product.Description
                        },
                        QuantityBought = x.QuantityBought,
                        OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                        DeleveredDate = x.Order.LastModifiedOn.ToLongDateString()

                    }).ToList(),
                    isDelivered = order[0].Order.isDelivered
                };
                productOrdersDtos.Add(productOrder);
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = productOrdersDtos.Select(x => new SalesDto
                {
                    OrderDtos = x.OrderDtos,
                    AmountPaid = x.OrderDtos.Sum(x => (x.ProductDto.Price * x.QuantityBought)),
                    // OrderedDate = x.OrderDtos
                    CustomerDto = x.CustomerDto,
                    AddressId = x.AddressDto.AddressId
                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetAllSales()
        {

            var sales = await _salesRepository.GetAllSales();
            if (sales.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = "no sales found",
                    Success = false
                };
            }
            List<ProductOrdersDto> productOrdersDtos = new List<ProductOrdersDto>();
            foreach (var sale in sales)
            {
                var order = await _productOrdersRepository.GetOrdersByIdAsync(sale.OrderId);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = order[0].Order.Customer.FullName,
                        Username = order[0].Order.Customer.User.Username,
                        PhoneNumber = order[0].Order.Customer.PhoneNumber,
                        Email = order[0].Order.Customer.User.Email,
                        ImageUrl = sale.Order.Customer.User.ProfileImage
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                        AdditionalDetails = order[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = order.Select(x => new OrderDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            ImageUrl = x.Product.ImageUrl,
                            isAvailable = x.Product.isAvailable,
                            Description = x.Product.Description
                        },
                        
                        QuantityBought = x.QuantityBought,
                        OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                        DeleveredDate = x.Order.LastModifiedOn.ToLongDateString(),

                    }).ToList(),
                    isDelivered = order[0].Order.isDelivered,
                    OrderId = sale.OrderId,
                };
                productOrdersDtos.Add(productOrder);
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = productOrdersDtos.Select(x => new SalesDto
                {
                    OrderDtos = x.OrderDtos,
                    AmountPaid = x.OrderDtos.Sum(x => (x.ProductDto.Price * x.QuantityBought)),
                    CustomerDto = x.CustomerDto,
                    AddressId = x.AddressDto.AddressId,
                    OrderId = x.OrderId
                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetSalesForThisYear()
        {

            var sales = await _salesRepository.GetThisYearSales();
            if (sales.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = "no sales found for this customer",
                    Success = false
                };
            }
            List<ProductOrdersDto> productOrdersDtos = new List<ProductOrdersDto>();
            foreach (var sale in sales)
            {
                var order = await _productOrdersRepository.GetOrdersByIdAsync(sale.OrderId);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = order[0].Order.Customer.FullName,
                        Username = order[0].Order.Customer.User.Username,
                        PhoneNumber = order[0].Order.Customer.PhoneNumber,
                        Email = order[0].Order.Customer.User.Email
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                        AdditionalDetails = order[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = order.Select(x => new OrderDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            ImageUrl = x.Product.ImageUrl,
                            isAvailable = x.Product.isAvailable,
                            Description = x.Product.Description
                        },
                        QuantityBought = x.QuantityBought,
                        OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                        DeleveredDate = x.Order.LastModifiedOn.ToLongDateString()

                    }).ToList(),
                    isDelivered = order[0].Order.isDelivered
                };
                productOrdersDtos.Add(productOrder);
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = productOrdersDtos.Select(x => new SalesDto
                {
                    OrderDtos = x.OrderDtos,
                    AmountPaid = x.OrderDtos.Sum(x => (x.ProductDto.Price * x.QuantityBought)),
                    // OrderedDate = x.OrderDtos
                    // DeliveredDate = sales.Select(x => x.CreatedOn.ToLongDateString()).SingleOrDefault(),
                    CustomerDto = x.CustomerDto,
                    AddressId = x.AddressDto.AddressId
                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetSalesForThisMonth()
        {

            var sales = await _salesRepository.GetThisMonthSales();
            if (sales.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = "no sales found for this customer",
                    Success = false
                };
            }
            List<ProductOrdersDto> productOrdersDtos = new List<ProductOrdersDto>();
            foreach (var sale in sales)
            {
                var order = await _productOrdersRepository.GetOrdersByIdAsync(sale.OrderId);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = order[0].Order.Customer.FullName,
                        Username = order[0].Order.Customer.User.Username,
                        PhoneNumber = order[0].Order.Customer.PhoneNumber,
                        Email = order[0].Order.Customer.User.Email
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                        AdditionalDetails = order[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = order.Select(x => new OrderDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            ImageUrl = x.Product.ImageUrl,
                            isAvailable = x.Product.isAvailable,
                            Description = x.Product.Description
                        },
                        QuantityBought = x.QuantityBought,
                        OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                        DeleveredDate = x.Order.LastModifiedOn.ToLongDateString()

                    }).ToList(),
                    isDelivered = order[0].Order.isDelivered
                };
                productOrdersDtos.Add(productOrder);
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = productOrdersDtos.Select(x => new SalesDto
                {
                    OrderDtos = x.OrderDtos,
                    AmountPaid = x.OrderDtos.Sum(x => (x.ProductDto.Price * x.QuantityBought)),
                    // OrderedDate = x.OrderDtos
                    // DeliveredDate = sales.Select(x => x.CreatedOn.ToLongDateString()).SingleOrDefault(),
                    CustomerDto = x.CustomerDto,
                    AddressId = x.AddressDto.AddressId
                }).ToList()
            };
        }
        // public async Task<SalesResponseModel> GetSalesByCustomerNameAndDateAsync(string name, DateTime dateOrded)
        // {
        //     var customer = await _customerRepository.GetAsync(x => x.User.Username == name);
        //     if (customer == null)
        //     {
        //         return new SalesResponseModel
        //         {
        //             Message = $"Customer with {name} is not registered on this app",
        //             Success = false
        //         };
        //     }
        //     var sales = await _salesRepository.GetSalesByCustomerIdAndDateAsync(customer.Id, dateOrded);
        //     if (sales == null)
        //     {
        //         return new SalesResponseModel
        //         {
        //             Message = "no sales found for this customer",
        //             Success = false
        //         };
        //     }
        //     return new SalesResponseModel
        //     {
        //         Message = "Sales found successfully",
        //         Success = true,
        //         Data = sales.Select(x => new SalesDto
        //         {
        //             QuantityBought = x.Order.QuantityBought,
        //             AmountPaid = x.AmountPaid,
        //             ProductDto = new ProductDto
        //             {
        //                 Name = x.Product.Name,
        //                 Price = x.Product.Price,
        //                 ImageUrl = x.Product.ImageUrl,
        //                 isAvailable = x.Product.isAvailable
        //             },

        //         }).ToList()
        //     };
        // }
        public async Task<OrdersResponseModel> GetSalesForTheMonthOnEachProduct(int month, int year)
        {
            var prod = await _productRepository.GetAllProductsAsync();
            if (prod.Count == 0)
            {
                return new OrdersResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<ProductOrders> monthlySales = new List<ProductOrders>();
            foreach (var item in prod)
            {
                var salesPerProduct = await _productOrdersRepository.GetAllDeleveredOrderByProductIdForTheMonthAsync(item.Id, month, year);
                decimal quantity = 0;
                foreach (var sal in salesPerProduct)
                {
                    quantity += sal.QuantityBought;
                }
                var productOders = new ProductOrders
                {
                    QuantityBought = salesPerProduct.Sum(x => x.QuantityBought),
                    Order = new Order
                    {
                    },
                    Product = new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        isAvailable = item.isAvailable,
                        Price = item.Price
                    }
                };
                monthlySales.Add(productOders);
            }
            return new OrdersResponseModel
            {
                Message = "Sales found Successfully",
                Success = true,
                Data = monthlySales.Select(x => new OrderDto
                {
                    AmountPaid = x.QuantityBought * x.Product.Price,
                    QuantityBought = x.QuantityBought,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                        Price = x.Product.Price
                    }
                }).ToList()
            };
        }
        public async Task<OrdersResponseModel> GetSalesForTheYearOnEachProduct(int year)
        {
            var prod = await _productRepository.GetAllProductsAsync();
            if (prod.Count == 0)
            {
                return new OrdersResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<ProductOrders> monthlySales = new List<ProductOrders>();
            foreach (var item in prod)
            {
                var salesPerProduct = await _productOrdersRepository.GetAllDeleveredOrderByProductIdForTheYearAsync(item.Id, year);
                decimal quantity = 0;
                foreach (var sal in salesPerProduct)
                {
                    quantity += sal.QuantityBought;
                }
                var productOders = new ProductOrders
                {
                    QuantityBought = quantity,
                    Order = new Order
                    {
                    },
                    Product = new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        isAvailable = item.isAvailable,
                        Price = item.Price
                    }
                };
                monthlySales.Add(productOders);
            }
            return new OrdersResponseModel
            {
                Message = "Sales found Successfully",
                Success = true,
                Data = monthlySales.Select(x => new OrderDto
                {
                    AmountPaid = x.QuantityBought * x.Product.Price,
                    QuantityBought = x.QuantityBought,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                        Price = x.Product.Price
                    }
                }).ToList()
            };
        }
        public async Task<ProfitResponseModel> CalculateThisMonthProfitAsync()
        {
            var rawMaterial = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync();
            var sales = await _salesRepository.GetTotalMonthlySalesAsync();
            return new ProfitResponseModel
            {
                Message = "Succesfully calculated",
                Success = true,
                Data = new ProfitDto
                {
                    Profit = sales - rawMaterial,
                    // Percentage = ((sales - rawMaterial) * 100) / rawMaterial
                }
            };
        }
        public async Task<ProfitResponseModel> CalculateMonthlyProfitAsync(int month, int year)
        {
            var products = await _productRepository.GetAllAsync();
            if (products.Count == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No products yet",
                    Success = false
                };
            }
            var expenses = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync(month, year);
            if (expenses == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No expense found for this month",
                    Success = false
                };
            }
            var sales = await _salesRepository.GetTotalMonthlySalesAsync(month, year);
            if (sales == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No sales found for this month",
                    Success = false
                };
            }
            return new ProfitResponseModel
            {
                Message = "Calulated Successfully",
                Success = true,
                Data = new ProfitDto
                {
                    Profit = sales - expenses,
                    Percentage = ((sales - expenses) * 100) / expenses
                }
            };
        }
        public async Task<SalesResponseModel> CalculateAllMonthlySalesAsync(int year)
        {
            List<decimal> percentagesales = new List<decimal>();
            int month = 1;
            for (int i = 1; i < 13; i++)
            {
                var sale = await _salesRepository.GetTotalMonthlySalesAsync(month, year);
                var rawMaterial = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync(month, year);
                if (rawMaterial + sale == 0)
                {
                    percentagesales.Add(0);
                }
                else
                {
                    percentagesales.Add(sale / (sale + rawMaterial) * 100);
                }
                month++;
            }

            return new SalesResponseModel
            {
                Message = "Succesfull",
                Success = true,
                Data = percentagesales.Select(x => new SalesDto
                {
                    AmountPaid = Math.Ceiling(x)
                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> CalculateAllMonthlyRawMaterialAsync(int year)
        {
            List<decimal> percentagerawMaterials = new List<decimal>();
            int month = 1;
            for (int i = 1; i < 13; i++)
            {
                var rawMaterial = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync(month, year);
                var sale = await _salesRepository.GetTotalMonthlySalesAsync(month, year);
                if (rawMaterial + sale == 0)
                {
                    percentagerawMaterials.Add(0);
                }
                else
                {
                    percentagerawMaterials.Add(rawMaterial / (sale + rawMaterial) * 100);
                }
                month++;
            }
            return new RawMaterialsResponseModel
            {
                Message = "Succesfull",
                Success = true,
                Data = percentagerawMaterials.Select(x => new RawMaterialDto
                {
                    Cost = Math.Ceiling(x)
                }).ToList()
            };
        }
        public async Task<ProfitResponseModel> CalculateThisYearProfitAsync()
        {
            var products = await _productRepository.GetAllAsync();
            if (products.Count == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No products yet",
                    Success = false
                };
            }
            var rawMaterial = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheYearAsync();
            var sales = await _salesRepository.GetTotalYearlySalesAsync();
            return new ProfitResponseModel
            {
                Data = new ProfitDto
                {
                    Profit = sales - rawMaterial,
                    Percentage = ((sales - rawMaterial) * 100) / rawMaterial
                }
            };
        }
        public async Task<ProfitResponseModel> CalculateYearlyProfitAsync(int year)
        {
            var products = await _productRepository.GetAllAsync();
            if (products.Count == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No products yet",
                    Success = false
                };
            }
            var expenses = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheYearAsync(year);
            if (expenses == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No expense found for this month",
                    Success = false
                };
            }
            var sales = await _salesRepository.GetTotalYearlySalesAsync(year);
            if (sales == 0)
            {
                return new ProfitResponseModel
                {
                    Message = "No sales found for this month",
                    Success = false
                };
            }
            return new ProfitResponseModel
            {
                Data = new ProfitDto
                {
                    Profit = sales - expenses,
                    Percentage = ((sales - expenses) * 100) / expenses
                }
            };
        }
        public async Task<SalesResponseModel> GetSalesByProductNameForTheMonth(int productId, int month, int year)
        {
            var prdct = await _productRepository.GetAsync(productId);
            if (prdct == null)
            {
                return new SalesResponseModel
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            var sales = await _productOrdersRepository.GetAllDeleveredOrderByProductIdForTheMonthAsync(productId, month, year);
            if (sales.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = $"No sales found for {prdct.Name}",
                    Success = false
                };
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    // QuantityBought = x.QuantityBought,
                    AmountPaid = x.QuantityBought * x.Product.Price,
                    AddressId = x.Order.AddressId,
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                        ImageUrl = x.Order.Customer.User.ProfileImage,
                    },
                }).ToList()
            };
        }
        public async Task<ProductsSaleResponseModel> GetSalesByProductNameForTheYear(int productId,int year)
        {
            var prdct = await _productRepository.GetAsync(productId);
            if (prdct == null)
            {
                return new ProductsSaleResponseModel
                {
                    Message = "Product not found",
                    Success = false
                };
            }
            var sales = await _productOrdersRepository.GetAllDeleveredOrderByProductIdForTheYearAsync(productId, year);
            if (sales.Count == 0)
            {
                return new ProductsSaleResponseModel
                {
                    Message = $"No sales found for {prdct.Name}",
                    Success = false
                };
            }
            return new ProductsSaleResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new ProductSaleDto
                {
                    QuantityBought = x.QuantityBought,
                    AmountPaid = x.QuantityBought * x.Product.Price,
                    AddressId = x.Order.AddressId,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeleveredDate = x.Order.LastModifiedOn.ToLongDateString(),
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                        ImageUrl = x.Order.Customer.User.ProfileImage,
                    },
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                        Price = x.Product.Price
                    }
                }).ToList()
            };
        }
        public async Task<ProfitResponseModel> CalculateNetProfitAsync(int year, int month, decimal extraExpenses)
        {
            var expense = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync(month, year);
            var sales = await _salesRepository.GetTotalMonthlySalesAsync(month, year);
            return new ProfitResponseModel
            {
                Message = "Net profit calculated successfully",
                Success = true,
                Data = new ProfitDto
                {
                    Profit = sales - (expense + extraExpenses)
                }
            };
        }
    }
}