using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ProductionResponseModel : BaseResponse
    {
        public ProductionDto Data { get; set; }
    }
    public class ProductionsResponseModel : BaseResponse
    {
        public List<ProductionDto> Data { get; set; }
    }
}