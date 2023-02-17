using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IRoleServices
    {
        Task<BaseResponse> CreateRole(CreateRoleRequestModel model);
        Task<RolesResponseModel> GetAllRoleAsync();
        Task<RoleResponseModel> GetRoleByUserId(int id);
        Task<BaseResponse> DeleteRole(int id);
    }
}