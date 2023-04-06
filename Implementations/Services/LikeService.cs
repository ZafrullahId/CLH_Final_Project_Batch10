using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        public LikeService(ILikeRepository likeRepository, ICustomerRepository customerRepository, IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _likeRepository = likeRepository;
            _customerRepository = customerRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse> CreateLike(CreateLikeRequestModel model)
        {
            var user = await _userRepository.GetAsync(model.UserId);
            var review = await _reviewRepository.GetAsync(x => x.Id == model.ReviewId);
            if (user == null || review == null)
            {
                return new BaseResponse
                {
                    Message = "Opps Somthing went wrong",
                    Success = false
                };
            }
            var exist = await _likeRepository.ExistsAsync(x => x.ReviewId == model.ReviewId && x.UserId == model.UserId);
            if (exist == false)
            {
                var like = new Like
                {
                    UserId = user.Id,
                    ReviewId = review.Id
                };
                await _likeRepository.CreateAsync(like);
                return new BaseResponse
                {
                    Message = "Successfuly liked",
                    Success = true
                };
            }
            var result = await UpdateLikeAsync(model.ReviewId, model.UserId);
            if (result.Success == true)
            {
                return new BaseResponse
                {
                    Message = "Success",
                    Success = true
                };
            }
            return new BaseResponse
            {
                Message = "Failed",
                Success = false
            };
        }
        public async Task<LikeResponseModel> GetLikesByReviewIdAsync(int reviwId)
        {
            var review = await _reviewRepository.GetAsync(reviwId);
            if (review == null)
            {
                return new LikeResponseModel
                {
                    Message = "Review not found",
                    Success = false
                };
            }
            var likes = await _likeRepository.GetLikesByReviewIdAsync(review.Id);
            return new LikeResponseModel
            {
                Message = "Likes found",
                Success = true,
                Data = new LikeDto
                {
                    numberOfLikes = likes.Count(),
                    CustomerDto = likes.Select(x => new CustomerDto
                    {
                        Id = x.User.Id
                    }).ToList()
                }
            };
        }
        public async Task<BaseResponse> UpdateLikeAsync(int reviwId, int userId)
        {
            var like = await _likeRepository.GetAsync(x => x.ReviewId == reviwId && x.UserId == userId);
            if (like == null)
            {
                return new BaseResponse
                {
                    Message = "Couldn't update",
                    Success = false
                };
            }
            if (like.IsDeleted == true)
            {
                like.IsDeleted = false;
            }
            else if (like.IsDeleted == false)
            {
                like.IsDeleted = true;
            }
            await _likeRepository.UpdateAsync(like);
            return new BaseResponse
            {
                Message = "like updated successfully",
                Success = true
            };
        }
    }
}