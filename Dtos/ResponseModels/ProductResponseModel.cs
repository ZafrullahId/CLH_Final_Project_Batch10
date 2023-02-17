using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class ProductResponseModel : BaseResponse
    {
        public ProductDto Data { get; set; }
    }
    public class ProductsResponseModel : BaseResponse
    {
        public List<ProductDto> Data { get; set; }
    }
}