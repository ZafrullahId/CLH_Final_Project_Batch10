using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{

    public class ProductOrdersServices : IProductOrdersServices
    {
        private readonly IProductOrdersRepository _productOrdersServices;
        public ProductOrdersServices(IProductOrdersRepository productOrdersRepository)
        {
            _productOrdersServices = productOrdersRepository;
        }
        public async Task<ProductsOrdersResponseModel> Orders()
        {
            var orders = await _productOrdersServices.GetAllOrders();
            if (orders == null)
            {
                return new ProductsOrdersResponseModel
                {
                    Message = "No Order yet",
                    Success = false
                };
            }
            return new ProductsOrdersResponseModel
            {
                Message = "Order found Successfully",
                Success = true,
                Data = orders.Select(x => new ProductOrdersDto
                {
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    ImageUrl = x.Product.ImageUrl,
                    isAvailable = x.Product.isAvailable,
                    AddressDto = new AddressDto
                    {
                        State = x.Order.Address.State,
                        Street = x.Order.Address.Street,
                        City = x.Order.Address.City,
                        PostalCode = x.Order.Address.PostalCode,
                    },
                    CustomerDto = new CustomerDto
                    {
                        FullName = x.Order.Customer.FullName,
                        PhoneNumber = x.Order.Customer.PhoneNumber
                    }
                }).ToList()
            };
        }
    }
}