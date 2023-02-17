using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ProductOrdersResponseModel : BaseResponse
    {
        public ProductOrdersDto Data { get; set; }
    }
    public class ProductsOrdersResponseModel : BaseResponse
    {
        public List<ProductOrdersDto> Data { get; set; }
    }
}