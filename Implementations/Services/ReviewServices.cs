using System;
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

    public class ReviewServices : IReviewServices
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ICustomerRepository _customerRepository;
        public ReviewServices(IReviewRepository reviewRepository, ICustomerRepository customerRepository)
        {
            _reviewRepository = reviewRepository;
            _customerRepository = customerRepository;
        }
        public async Task<BaseResponse> CreateRiview(CreateReviewRequestModel model, int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer not found",
                    Success = false,
                };
            }
            var review = new Review
            {
                Text = model.Text,
                CustomerId = customer.Id,
                Seen = false
            };
            await _reviewRepository.CreateAsync(review);
            return new BaseResponse
            {
                Message = "Successfully Commented",
                Success = true
            };
        }
        public async Task<ReviewsResponseModel> GetAllReviewsByCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new ReviewsResponseModel
                {
                    Message = "Customer not found",
                    Success = false,
                };
            }
            var reviews = await _reviewRepository.GetReviewsByCustomerIdAsync(customer.Id);
            if (reviews.Count == 0)
            {
                return new ReviewsResponseModel
                {
                    Message = "Review not found",
                    Success = false
                };
            }
            return new ReviewsResponseModel
            {
                Message = "Reviews found",
                Success = true,
                Data = reviews.Select(x => new ReviewDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    FullName = x.Customer.FullName,
                    ImageUrl = x.Customer.User.ProfileImage,
                    Seen = x.Seen
                }).ToList()
            };
        }
        public async Task<ReviewResponseModel> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if(review == null)
            {
                return new ReviewResponseModel
                {
                    Message = "Review not found",
                    Success = false
                };
            }
            return new ReviewResponseModel
            {
                Message = "Review found successfully",
                Success = true,
                Data = new ReviewDto
                {
                    Id = review.Id,
                    Text = review.Text,
                    FullName = review.Customer.FullName,
                    ImageUrl = review.Customer.User.ProfileImage,
                }
            };
        }
        public async Task<ReviewsResponseModel> GetAllReviewsAsync()
        {
            var review = await _reviewRepository.GetAllReviewsAsync();
            if (review.Count == 0)
            {
                return new ReviewsResponseModel
                {
                    Message = "no Reviews yet",
                    Success = false
                };
            }
            foreach (var item in review)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.PostedTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
                
            }
            return new ReviewsResponseModel
            {
                Message = "Reviews found",
                Success = true,
                Data = review.Select(x => new ReviewDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    FullName = x.Customer.FullName,
                    ImageUrl = x.Customer.User.ProfileImage,
                    Seen = x.Seen,
                    PostedTime = x.PostedTime
                }).ToList()
            };
        }
        public async Task<ReviewsResponseModel> GetAllUnSeenReviewsAsync()
        {
            double s = 2;
            int x = (int) s;
            var reviews = await _reviewRepository.GetAllUnseenReviewsAsync();
            if (reviews.Count == 0)
            {
                return new ReviewsResponseModel
                {
                    Message = "no unseen Reviews yet",
                    Success = false
                };
            }
            foreach (var item in reviews)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.PostedTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.PostedTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
                
            }
            return new ReviewsResponseModel
            {
                Message = "Reviews found",
                Success = true,
                Data = reviews.Select(x => new ReviewDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    ImageUrl = x.Customer.User.ProfileImage,
                    FullName = x.Customer.FullName,
                    PostedTime = x.PostedTime
                                       
                }).ToList()
            };
        }
        public async Task<BaseResponse> UpdateReviewStatusAsync(int id)
        {
            var review = await _reviewRepository.GetAsync(x => x.Id == id);
            if (review == null)
            {
                return new BaseResponse
                {
                    Message = "review not found",
                    Success = false
                };
            }
            review.Seen = true;
            await _reviewRepository.UpdateAsync(review);
            return new BaseResponse
            {
                Message = "Review updated successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> UpdateAll()
        {
            var reviews = await _reviewRepository.GetAllUnseenReviewsAsync();
            if(reviews.Count == 0)
            {
                return new BaseResponse
                {
                    Message = "No reviews to be updated",
                    Success = false
                };
            }
            foreach (var item in reviews)
            {
                item.Seen = true;
                await _reviewRepository.UpdateAsync(item);
            }
            return new BaseResponse
            {
                Message = "Updated all",
                Success = true
            };
        }
        public async Task<BaseResponse> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetAsync(id);
            if(review == null)
            {
                return new BaseResponse
                {
                    Message = "Review  not",
                    Success = false
                };
            }
            review.IsDeleted = true;
            await _reviewRepository.UpdateAsync(review);
            return new BaseResponse
            {
                Message = "Review deleted Successfully",
                Success = true
            };
        }
    }
}