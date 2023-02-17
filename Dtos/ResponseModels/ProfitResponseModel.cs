using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ProfitResponseModel : BaseResponse
    {
        public ProfitDto Data { get; set; }
    }
    public class ProfitsResponseModel : BaseResponse
    {
        public List<ProfitDto> Data { get; set; }
    }
}