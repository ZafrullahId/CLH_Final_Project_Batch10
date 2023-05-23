using System.Threading.Tasks;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IVerificationCodeService
    {
        Task<BaseResponse> UpdateVeryficationCodeAsync(int id);
        Task<BaseResponse> VerifyCode(int id, int verificationcode);
    }
}