using Dansnom.Contracts;
using Dansnom.Entities.Identity;

namespace Dansnom.Entities
{
    public class Admin : AuditableEntity
    {
        public string FullName {get; set;}
        public string PhoneNumber { get; set; }
        public int UserId {get; set;}
        public User User {get;set;}
    }
}