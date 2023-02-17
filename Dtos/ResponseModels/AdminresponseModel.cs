using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class AdminResponseModel : BaseResponse
    {
        public AdminDto Data { get; set; }
    }
    public class AdminsResponseModel : BaseResponse
    {
        public List<AdminDto> Data { get; set; }
    }
}