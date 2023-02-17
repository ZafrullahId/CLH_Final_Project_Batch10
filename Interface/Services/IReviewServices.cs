using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;

namespace Dansnom.Interface.Services
{
    public interface IReviewServices
    {
        Task<BaseResponse> CreateRiview(CreateReviewRequestModel model, int id);
        Task<ReviewsResponseModel> GetAllReviewsByCustomerAsync(int id);
        Task<ReviewsResponseModel> GetAllReviewsAsync();
        Task<ReviewsResponseModel> GetAllUnSeenReviewsAsync();
        Task<BaseResponse> UpdateReviewStatusAsync(int id);
        Task<BaseResponse> DeleteReviewAsync(int id);
        Task<ReviewResponseModel> GetReviewByIdAsync(int id);
        Task<BaseResponse> UpdateAll();
    }
}