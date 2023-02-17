using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos.RequestModel
{
    public class UpdateCustomerRequestModel
    {
        public string Username {get; set;}
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}