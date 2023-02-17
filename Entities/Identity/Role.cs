using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities.Identity
{
    public class Role : AuditableEntity
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public List<UserRole> UserRoles {get; set;} = new List<UserRole>();
    }
}