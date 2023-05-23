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

    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISalesServices _salesServices;
        private readonly IProductOrdersRepository _productOrdersRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly ICartRepository _cartRepository;
        public OrderServices(IOrderRepository orderRepository, ICustomerRepository customerRepository, ISalesServices salesServices, IProductOrdersRepository productOrdersRepository, IProductRepository productRepository, IProductionRepository productionRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _salesServices = salesServices;
            _productOrdersRepository = productOrdersRepository;
            _productRepository = productRepository;
            _productionRepository = productionRepository;
            _cartRepository = cartRepository;
        }
        public async Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model, int userId)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId);
            foreach (var productId in model.request)
            {
                var product = await _productRepository.GetAsync(productId.ProductId);
                if (product == null)
                {
                    return new BaseResponse
                    {
                        Message = "Product not found",
                        Success = false
                    };
                }
            }
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer not found",
                    Success = false
                };
            }

            var productions = await _productionRepository.GetAllApprovedProduction();
            foreach (var order in model.request)
            {
                bool isRemaining = false;
                decimal quantityOrdered = order.QuantityBought;
                foreach (var item in productions)
                {
                    var production = await _productionRepository.GetProduction(item.Id, order.ProductId);
                    if (production != null && production.QuantityRemaining > 0 && quantityOrdered > production.QuantityRemaining)
                    {
                        quantityOrdered -= production.QuantityRemaining;
                        production.QuantityRemaining = 0;
                    }
                    else if (production != null && production.QuantityRemaining >= quantityOrdered)
                    {
                        production.QuantityRemaining -= (int)quantityOrdered;
                        quantityOrdered = 0;
                    }
                    if (quantityOrdered == 0)
                    {
                        isRemaining = true;
                        await _productionRepository.UpdateAsync(production);
                        break;
                    }
                    else
                    {
                        isRemaining = false;
                    }
                    continue;
                }
                if (isRemaining == false)
                {
                    return new BaseResponse
                    {
                        Message = $"Quantiy remaining is not up to the quantity you request for",
                        Success = false
                    };
                }

            }
            
            var ord = new Order
            {
                CustomerId = customer.Id,
                isDelivered = false,
                Address = new Address
                {
                    PostalCode = model.PostalCode,
                    City = model.City,
                    State = model.State,
                    Street = model.Street,
                    AdditionalDetails = model.AdditionalDetails
                }
            };
            var cord = await _orderRepository.CreateAsync(ord);
            foreach (var item in model.request)
            {
                var productOrders = new ProductOrders
                {
                    ProductId = item.ProductId,
                    OrderId = cord.Id,
                    QuantityBought = item.QuantityBought,
                };
                await _productOrdersRepository.CreateAsync(productOrders);
            }
            return new BaseResponse
            {
                Message = "Successfully Ordered",
                Success = true
            };
        }
        public async Task<ProductOrdersResponseModel> GetOrderByIdAsync(int id)
        {
            var order = await _productOrdersRepository.GetOrdersByIdAsync(id);
            if (order == null)
            {
                return new ProductOrdersResponseModel
                {
                    Message = "Order not found",
                    Success = false
                };
            }
            return new ProductOrdersResponseModel
            {
                Message = "Order found Successfully",
                Success = true,
                Data = new ProductOrdersDto
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
                        AddressId = order[0].Order.AddressId,
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
                        AmountPaid = x.QuantityBought * x.Product.Price,
                        
                    }).ToList(),
                    isDelivered = order[0].Order.isDelivered,
                    NetAmount = order.Sum(x => x.QuantityBought * x.Product.Price)
                }};
        }
        public async Task<ProductsOrdersResponseModel> GetOrdersByCustomerIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "Customer not found",
                    Success = false
                };
            }
            // var orders = await _orderRepository.Get
            var orders = await _orderRepository.GetOrderByCustomerId(customer.Id);
            if (orders.Count == 0)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "Order not found",
                    Success = false
                };
            }
            List<ProductOrdersDto> productOrdersDtos = new List<ProductOrdersDto>();
            foreach (var ord in orders)
            {
                var order = await _productOrdersRepository.GetOrdersByIdAsync(ord.Id);
                var productOders = new ProductOrdersDto
                {
                     CustomerDto = new CustomerDto
                    {
                        FullName = ord.Customer.FullName,
                        Username = ord.Customer.User.Username,
                        PhoneNumber = ord.Customer.PhoneNumber,
                        Email = ord.Customer.User.Email,
                        ImageUrl = ord.Customer.User.ProfileImage
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = ord.AddressId,
                        State = ord.Address.State,
                        City = ord.Address.City,
                        Street = ord.Address.Street,
                        PostalCode = ord.Address.PostalCode,
                        AdditionalDetails = ord.Address.AdditionalDetails
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
                        AmountPaid = x.QuantityBought * x.Product.Price
                        
                    }).ToList(),
                    isDelivered = ord.isDelivered
                };
                productOrdersDtos.Add(productOders);
            }
            return new ProductsOrdersResponseModel
            {
                Message = "Order found Successfully",
                Success = true,
                Data = productOrdersDtos.Select(x => new ProductOrdersDto
                {
                    CustomerDto = x.CustomerDto,
                    AddressDto = x.AddressDto,
                    isDelivered = x.isDelivered,
                    OrderDtos = x.OrderDtos
                }).ToList()
            };
        }
        public async Task<ProductsOrdersResponseModel> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders.Count == 0)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            List<ProductOrdersDto> allOrders = new List<ProductOrdersDto>();
            foreach (var order in orders)
            {
                var productOrders = await _productOrdersRepository.GetOrdersByIdAsync(order.Id);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = productOrders[0].Order.Customer.FullName,
                        Username = productOrders[0].Order.Customer.User.Username,
                        PhoneNumber = productOrders[0].Order.Customer.PhoneNumber,
                        Email = productOrders[0].Order.Customer.User.Email,
                        ImageUrl = productOrders[0].Order.Customer.User.ProfileImage
                    },
                    AddressDto = new AddressDto
                    {
                        AddressId = order.AddressId,
                        State = productOrders[0].Order.Address.State,
                        City = productOrders[0].Order.Address.City,
                        Street = productOrders[0].Order.Address.Street,
                        PostalCode = productOrders[0].Order.Address.PostalCode,
                        AdditionalDetails = productOrders[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = productOrders.Select(x => new OrderDto
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
                    }).ToList(),
                    isDelivered = productOrders[0].Order.isDelivered,
                    OrderId = order.Id
                };
                allOrders.Add(productOrder);
            }
            return new ProductsOrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = allOrders.Select(x => new ProductOrdersDto
                {
                    AddressDto = x.AddressDto,
                    CustomerDto = x.CustomerDto,
                    OrderDtos = x.OrderDtos,
                    isDelivered = x.isDelivered,
                    NetAmount = x.OrderDtos.Sum(x => x.QuantityBought * x.ProductDto.Price),
                    OrderId = x.OrderId
                }).ToList()
            };
        }
        public async Task<ProductsOrdersResponseModel> GetAllDeleveredOrders()
        {
            var orders = await _orderRepository.GetAllDeleveredOrdersAsync();
            if (orders.Count == 0)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            List<ProductOrdersDto> allOrders = new List<ProductOrdersDto>();
            foreach (var order in orders)
            {
                var productOrders = await _productOrdersRepository.GetOrdersByIdAsync(order.Id);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = productOrders[0].Order.Customer.FullName,
                        Username = productOrders[0].Order.Customer.User.Username,
                        PhoneNumber = productOrders[0].Order.Customer.PhoneNumber,
                        Email = productOrders[0].Order.Customer.User.Email,
                        ImageUrl = productOrders[0].Order.Customer.User.ProfileImage
                    },
                    AddressDto = new AddressDto
                    {
                        State = productOrders[0].Order.Address.State,
                        City = productOrders[0].Order.Address.City,
                        Street = productOrders[0].Order.Address.Street,
                        PostalCode = productOrders[0].Order.Address.PostalCode,
                        AdditionalDetails = productOrders[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = productOrders.Select(x => new OrderDto
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
                        OrderedDate = x.Order.CreatedOn.ToLongDateString()
                    }).ToList(),
                    isDelivered = productOrders[0].Order.isDelivered,
                    OrderId = order.Id
                };
                allOrders.Add(productOrder);
            }
            return new ProductsOrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = allOrders.Select(x => new ProductOrdersDto
                {
                    AddressDto = x.AddressDto,
                    CustomerDto = x.CustomerDto,
                    OrderDtos = x.OrderDtos,
                    isDelivered = x.isDelivered,
                    NetAmount = x.OrderDtos.Sum(x => x.QuantityBought * x.ProductDto.Price),
                    OrderId = x.OrderId
                }).ToList()
            };
        }
        public async Task<ProductsOrdersResponseModel> GetAllUnDeleveredOrders()
        {
            var orders = await _orderRepository.GetAllUnDeleveredOrdersAsync();
            if (orders.Count == 0)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            List<ProductOrdersDto> allOrders = new List<ProductOrdersDto>();
            foreach (var order in orders)
            {
                var productOrders = await _productOrdersRepository.GetOrdersByIdAsync(order.Id);
                var productOrder = new ProductOrdersDto
                {
                    CustomerDto = new CustomerDto
                    {
                        FullName = productOrders[0].Order.Customer.FullName,
                        Username = productOrders[0].Order.Customer.User.Username,
                        PhoneNumber = productOrders[0].Order.Customer.PhoneNumber,
                        Email = productOrders[0].Order.Customer.User.Email,
                        ImageUrl = productOrders[0].Order.Customer.User.ProfileImage,
                    },
                    AddressDto = new AddressDto
                    {
                        State = productOrders[0].Order.Address.State,
                        City = productOrders[0].Order.Address.City,
                        Street = productOrders[0].Order.Address.Street,
                        PostalCode = productOrders[0].Order.Address.PostalCode,
                        AdditionalDetails = productOrders[0].Order.Address.AdditionalDetails
                    },
                    OrderDtos = productOrders.Select(x => new OrderDto
                    {
                        ProductDto = new ProductDto
                        {
                            Name = x.Product.Name,
                            Price = x.Product.Price,
                            ImageUrl = x.Product.ImageUrl,
                            isAvailable = x.Product.isAvailable,
                            Description = x.Product.Description,
                        },
                        QuantityBought = x.QuantityBought,
                        OrderedDate = x.Order.CreatedOn.ToLongDateString()
                    }).ToList(),
                    isDelivered = productOrders[0].Order.isDelivered,
                    OrderId = order.Id
                };
                allOrders.Add(productOrder);
            }
            return new ProductsOrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = allOrders.Select(x => new ProductOrdersDto
                {
                    AddressDto = x.AddressDto,
                    CustomerDto = x.CustomerDto,
                    OrderDtos = x.OrderDtos,
                    isDelivered = x.isDelivered,
                    NetAmount = x.OrderDtos.Sum(x => x.QuantityBought * x.ProductDto.Price),
                    OrderId = x.OrderId
                }).ToList()
            };
        }
        public async Task<BaseResponse> UpdateOrder(int id)
        {
            var order = await _orderRepository.GetAsync(x => x.Id == id);
            if (order == null)
            {
                return new BaseResponse
                {
                    Message = "Order not found",
                    Success = false
                };
            }
            if (order.isDelivered == true)
            {
                return new BaseResponse
                {
                    Message = "Order already delevered",
                    Success = false
                };
            }
            order.isDelivered = true;
            order.LastModifiedOn = DateTime.Now;
            var sale = await _salesServices.CreateSales(id);
            if (sale.Success == false)
            {
                return new BaseResponse
                {
                    Message = "Sales not created",
                    Success = false
                };
            }
            await _orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Message = "Order Updated Successfully, Sales created and wallet funded",
                Success = true
            };
        }

    }
}