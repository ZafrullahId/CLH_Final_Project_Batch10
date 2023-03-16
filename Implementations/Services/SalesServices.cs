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
        public SalesServices(ISalesRepository salesRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IWalletServices walletServices, IProductOrdersRepository productOrdersRepository, IRawMaterialRepository expensesRepository, IProductionRepository productionRepository)
        {
            _salesRepository = salesRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _walletServices = walletServices;
            _productOrdersRepository = productOrdersRepository;
            _ramMaterialRepository = expensesRepository;
            _productionRepository = productionRepository;
        }
        public async Task<BaseResponse> CreateSales(int id)
        {
            var productOrders = await _productOrdersRepository.GetOrderById(id);
            if (productOrders == null)
            {
                return new BaseResponse
                {
                    Message = "Product not found",
                    Success = false
                };
            }

            var sales = new Sales
            {
                OrderId = productOrders.OrderId,
                ProductId = productOrders.ProductId,
                AmountPaid = productOrders.Order.QuantityBought * productOrders.Product.Price
            };
            var sale = await _salesRepository.CreateAsync(sales);
            var wallet = await _walletServices.FundWallet(productOrders.Order.QuantityBought * productOrders.Product.Price);

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
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeliveredDate = x.CreatedOn.ToLongDateString(),
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                    }

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
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeliveredDate = x.CreatedOn.ToLongDateString(),
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                    }

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
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeliveredDate = x.CreatedOn.ToLongDateString(),
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                    }

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
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeliveredDate = x.CreatedOn.ToLongDateString(),
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                    }

                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetSalesByCustomerNameAndDateAsync(string name, DateTime dateOrded)
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
            var sales = await _salesRepository.GetSalesByCustomerIdAndDateAsync(customer.Id, dateOrded);
            if (sales == null)
            {
                return new SalesResponseModel
                {
                    Message = "no sales found for this customer",
                    Success = false
                };
            }
            return new SalesResponseModel
            {
                Message = "Sales found successfully",
                Success = true,
                Data = sales.Select(x => new SalesDto
                {
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    ProductDto = new ProductDto
                    {
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.ImageUrl,
                        isAvailable = x.Product.isAvailable
                    },

                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetSalesForTheMonthOnEachProduct(int month, int year)
        {
            var prod = await _productRepository.GetAllProductsAsync();
            if (prod.Count == 0)
            {
                return new SalesResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<Sales> monthlySales = new List<Sales>();

            foreach (var item in prod)
            {
                var salesPerProduct = await _salesRepository.GetSalesForTheMonthAsync(item.Id, month, year);

                var sale = new Sales
                {
                    AmountPaid = salesPerProduct.Sum(x => x.AmountPaid),
                    Order = new Order
                    {
                        QuantityBought = salesPerProduct.Sum(x => x.Order.QuantityBought)
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
                monthlySales.Add(sale);
            }
            return new SalesResponseModel
            {
                Message = "Sales found Successfully",
                Success = true,
                Data = monthlySales.Select(x => new SalesDto
                {
                    AmountPaid = x.AmountPaid,
                    QuantityBought = x.Order.QuantityBought,
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
        public async Task<SalesResponseModel> GetSalesForTheYearOnEachProduct(int year)
        {
            var prod = await _productRepository.GetAllProductsAsync();
            if (prod == null)
            {
                return new SalesResponseModel
                {
                    Message = "No Products found",
                    Success = true
                };
            }
            List<Sales> yearlySales = new List<Sales>();

            foreach (var item in prod)
            {
                var salesPerProduct = await _salesRepository.GetSalesForTheYearAsync(item.Id, year);

                var sale = new Sales
                {
                    AmountPaid = salesPerProduct.Sum(x => x.AmountPaid),
                    Order = new Order
                    {
                        QuantityBought = salesPerProduct.Sum(x => x.Order.QuantityBought)
                    },
                    Product = new Product
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl
                    }
                };
                yearlySales.Add(sale);
            }
            return new SalesResponseModel
            {
                Message = "Sales found Successfully",
                Success = true,
                Data = yearlySales.Select(x => new SalesDto
                {
                    AmountPaid = x.AmountPaid,
                    QuantityBought = x.Order.QuantityBought,
                    ProductDto = new ProductDto
                    {
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        ImageUrl = x.Product.ImageUrl
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
            var sales = await _salesRepository.GetSalesForTheMonthAsync(prdct.Id, month, year);
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
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                        ImageUrl = x.Order.Customer.User.ProfileImage,
                    }
                }).ToList()
            };
        }
        public async Task<SalesResponseModel> GetSalesByProductNameForTheYear(int productId,int year)
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
            var sales = await _salesRepository.GetSalesForTheYearAsync(prdct.Id, year);
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
                    QuantityBought = x.Order.QuantityBought,
                    AmountPaid = x.AmountPaid,
                    AddressId = x.Order.AddressId,
                    OrderedDate = x.Order.CreatedOn.ToLongDateString(),
                    DeliveredDate = x.CreatedOn.ToLongDateString(),
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber,
                        ImageUrl = x.Order.Customer.User.ProfileImage,
                    },
                    ProductDto = new ProductDto
                    {
                        ProductId = prdct.Id
                    }
                }).ToList()
            };
        }
        public async Task<ProfitResponseModel> CalculateNetProfitAsync(int year, int month, decimal extraExpenses)
        {
            var expense = await _ramMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync(month,year);
            var sales = await _salesRepository.GetTotalMonthlySalesAsync(month,year);
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