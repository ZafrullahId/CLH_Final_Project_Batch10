using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface ILikeService
    {
        Task<BaseResponse> CreateLike(CreateLikeRequestModel model);
        Task<LikeResponseModel> GetLikesByReviewIdAsync(int reviwId);
    }

}