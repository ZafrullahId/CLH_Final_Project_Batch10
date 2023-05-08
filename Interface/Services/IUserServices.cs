using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IUserServices
    {
        Task<UserResponseModel> Login(LoginRequestModel model);
        Task<UsersResponseModel> GetUsersByRoleAsync(string role);
        Task<UserResponseModel> GetUserByTokenAsync(string token);
    }
}