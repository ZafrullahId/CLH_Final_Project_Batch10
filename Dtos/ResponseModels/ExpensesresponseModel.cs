using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class RawMaterialResponseModel : BaseResponse
    {
        public RawMaterialDto Data { get; set; }
    }
    public class RawMaterialsResponseModel : BaseResponse
    {
        public List<RawMaterialDto> Data { get; set; }
    }
}