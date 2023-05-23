using System.Threading.Tasks;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Dtos.RequestModel;

namespace Dansnom.Interface.Services
{
    public interface ICustomerServices
    {
        Task<BaseResponse> DeleteAsync(int id);
        Task<CustomerReponseModel> GetByidAsnc(int id);
        Task<CustomerReponseModel> RegisterAsync(CreateCustomerRequestModel model);
        Task<CustomerReponseModel> UpdateProfile(UpdateCustomerRequestModel model, int id);
    }
}