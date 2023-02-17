using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos.RequestModel
{
    public class CreateAdminRequestModel
    {
        public string Username {get; set;}
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public string Email {get; set;}  
        public string Password { get; set; }
        public IFormFile profileImage { get; set; }
        public string Role {get;set;}
    }
}