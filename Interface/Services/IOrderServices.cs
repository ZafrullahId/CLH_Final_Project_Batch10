using System.Collections.Generic;
using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IOrderServices
    {
        Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model, int userId);
        Task<ProductsOrdersResponseModel> GetAllDeleveredOrders();
        Task<ProductsOrdersResponseModel> GetAllOrders();
        Task<ProductsOrdersResponseModel> GetAllUnDeleveredOrders();
        Task<ProductOrdersResponseModel> GetOrderByIdAsync(int id);
        Task<BaseResponse> UpdateOrder(int id);
        Task<ProductsOrdersResponseModel> GetOrdersByCustomerIdAsync(int id);
    }
}