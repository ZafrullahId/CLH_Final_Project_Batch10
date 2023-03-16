using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dansnom.Contracts;
using Dansnom.Entities.Identity;

namespace Dansnom.Entities
{
    public class Customer : AuditableEntity
    {
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public int UserId {get;set;} 
        public User User {get;set;}  
        public List<Order> Orders = new List<Order>();
        public List<Review> Review { get; set; } = new List<Review>();
        public List<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
       

    }
}