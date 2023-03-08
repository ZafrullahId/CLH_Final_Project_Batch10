using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IAdminServices
    {
        Task<BaseResponse> AddAdmin(CreateAdminRequestModel model);
        Task<BaseResponse> DeleteAdmin(int Id);
        Task<AdminResponseModel> FindAdminAsync(int id);
        Task<AdminsResponseModel> GetAllAdmins();
        Task<AdminResponseModel> UpdateProfile(UpdateAdminRequestModel model, int id);
        Task<BaseResponse> CompleteRegistration(CompleteManagerRegistrationRequestModel model);
    }
}