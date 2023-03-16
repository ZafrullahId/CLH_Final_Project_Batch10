using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class LikeResponseModel : BaseResponse
    {
        public LikeDto Data { get; set; }
    }
    public class LikesResponseModel : BaseResponse
    {
        public List<LikeDto> Data { get; set; }
    }
}