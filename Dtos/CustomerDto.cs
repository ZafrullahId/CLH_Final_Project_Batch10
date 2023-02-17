using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Username {get; set;}
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }
}