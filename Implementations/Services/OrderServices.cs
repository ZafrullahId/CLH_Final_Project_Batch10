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
        private readonly IProductOrdersRepository _productOrdersServices;
        private readonly IProductRepository _productRepository;
         private readonly IProductionRepository _productionRepository;
        public OrderServices(IOrderRepository orderRepository, ICustomerRepository customerRepository,ISalesServices salesServices,IProductOrdersRepository productOrdersRepository,IProductRepository productRepository,IProductionRepository productionRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _salesServices = salesServices;
            _productOrdersServices = productOrdersRepository;
            _productRepository = productRepository;
            _productionRepository = productionRepository;
        }
        public async Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model, int id,int Productid)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            var product = await _productRepository.GetAsync(Productid);
            if (customer == null || product == null)
            {
                return new BaseResponse
                {
                    Message = "Something went wrong",
                    Success = false
                };
            }
            var productions = await _productionRepository.GetAllApprovedProduction();
            bool isRemaining = false;
            decimal quantityOrdered = model.QuantityBought;
            foreach (var item in productions)
            {
                var production = await _productionRepository.GetProduction(item.Id, Productid);
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
                if(quantityOrdered == 0)
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
            var order = new Order
            {
                CustomerId = customer.Id,
                QuantityBought = model.QuantityBought,
                isDelivered = false,
                Address = new Address
                {
                    PostalCode = model.PostalCode,
                    City = model.City,
                    State = model.State,
                    Street = model.Street
                }
            };
            var ord = await _orderRepository.CreateAsync(order);
            var productOrders = new ProductOrders
            {
                ProductId = product.Id,
                OrderId = ord.Id
            };
            await _productOrdersServices.CreateAsync(productOrders);
            return new BaseResponse
            {
                Message = "Successfully Ordered",
                Success = true
            };
        }
        public async Task<OrderResponseModel> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return new OrderResponseModel
                {
                    Message = "Order not found",
                    Success = false
                };
            }
            return new OrderResponseModel
            {
                Message = "Order found Successfully",
                Success = true,
                Data = new OrderDto
                {
                    isDelivered = order.isDelivered,
                    QuantityBought = order.QuantityBought,
                    AddressDto = new AddressDto
                    {
                        City = order.Address.City,
                        State = order.Address.State,
                        PostalCode = order.Address.PostalCode,
                        Street = order.Address.Street
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = order.Customer.FullName,
                        Username = order.Customer.User.Username,
                        PhoneNumber = order.Customer.PhoneNumber,
                        Email = order.Customer.User.Email
                    }
                }
            };
        }
        public async Task<OrdersResponseModel> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrderAsync();
            if (orders == null)
            {
                return new OrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            return new OrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = orders.Select(x => new OrderDto
                {
                    QuantityBought = x.QuantityBought,
                    isDelivered = x.isDelivered,
                    AddressDto = new AddressDto
                    {
                        Street = x.Address.Street,
                        State = x.Address.State,
                        City = x.Address.City,
                        PostalCode = x.Address.PostalCode,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Customer.FullName,
                        Username = x.Customer.User.Username,
                        PhoneNumber = x.Customer.PhoneNumber,
                        Email = x.Customer.User.Email
                    }
                }).ToList()
            };
        }
        public async Task<OrdersResponseModel> GetAllDeleveredOrders()
        {
            var orders = await _orderRepository.GetAllDeleveredOrderAsync();
            if (orders == null)
            {
                return new OrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            return new OrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = orders.Select(x => new OrderDto
                {
                    QuantityBought = x.QuantityBought,
                    isDelivered = x.isDelivered,
                    AddressDto = new AddressDto
                    {
                        Street = x.Address.Street,
                        State = x.Address.State,
                        City = x.Address.City,
                        PostalCode = x.Address.PostalCode,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Customer.FullName,
                        Username = x.Customer.User.Username,
                        PhoneNumber = x.Customer.PhoneNumber,
                        Email = x.Customer.User.Email
                    }
                }).ToList()
            };
        }
        public async Task<OrdersResponseModel> GetAllUnDeleveredOrders()
        {
            var orders = await _orderRepository.GetAllUnDeleveredOrderAsync();
            if (orders == null)
            {
                return new OrdersResponseModel
                {
                    Message = "Orders not found",
                    Success = false
                };
            }
            return new OrdersResponseModel
            {
                Message = "Orders found successfully",
                Success = true,
                Data = orders.Select(x => new OrderDto
                {
                    QuantityBought = x.QuantityBought,
                    isDelivered = x.isDelivered,
                    AddressDto = new AddressDto
                    {
                        Street = x.Address.Street,
                        State = x.Address.State,
                        City = x.Address.City,
                        PostalCode = x.Address.PostalCode,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Customer.FullName,
                        Username = x.Customer.User.Username,
                        PhoneNumber = x.Customer.PhoneNumber,
                        Email = x.Customer.User.Email
                    }
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
            order.isDelivered = true;
            var sale = await _salesServices.CreateSales(id);
            if(sale.Success == false)
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