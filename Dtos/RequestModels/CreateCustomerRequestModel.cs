using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos.RequestModel
{
    public  class CreateCustomerRequestModel
    {
        public string Username {get; set;}
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}