using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ReviewResponseModel : BaseResponse
    {
        public ReviewDto Data { get; set; }
    }
    public class ReviewsResponseModel : BaseResponse
    {
        public List<ReviewDto> Data { get; set; }
    }
}