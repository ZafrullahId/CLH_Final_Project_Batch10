using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IOrderServices
    {
        Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model, int id, int Productid);
        Task<OrdersResponseModel> GetAllDeleveredOrders();
        Task<OrdersResponseModel> GetAllOrders();
        Task<OrdersResponseModel> GetAllUnDeleveredOrders();
        Task<OrderResponseModel> GetOrderByIdAsync(int id);
        Task<BaseResponse> UpdateOrder(int id);
    }
}