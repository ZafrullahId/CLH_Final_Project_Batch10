using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities.Identity
{
    public class User : AuditableEntity
    {
        public string Email {get; set;}  
        public string Password { get; set; }     
        public string Username {get; set;}
        public string ProfileImage { get; set; }
        public Customer Customer {get;set;}
        public Admin Admin {get; set;}
        public List<UserRole> UserRoles {get; set;} = new List<UserRole>();
    }
}