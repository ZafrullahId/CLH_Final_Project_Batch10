using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class CategoryResponseModel : BaseResponse
    {
        public CategoryDto Data { get; set; }
    }
    public class CategoriesResponseModel : BaseResponse
    {
        public List<CategoryDto> Data { get; set; }
    }
}