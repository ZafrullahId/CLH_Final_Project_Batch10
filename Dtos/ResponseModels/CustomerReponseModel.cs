using System.Collections.Generic;

namespace Dansnom.Dtos.ResponseModel
{
    public class CustomerReponseModel : BaseResponse
    {
        public CustomerDto Data { get; set; }
    }
    public class CustomersReponseModel : BaseResponse
    {
        public List<CustomerDto> Data { get; set; }
    }
}