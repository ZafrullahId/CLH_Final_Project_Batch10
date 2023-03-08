using System.Collections.Generic;
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
        public List<RawMaterial> RawMaterial { get; set; } = new List<RawMaterial>();
        public List<Production> Production { get; set; } = new List<Production>();
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}