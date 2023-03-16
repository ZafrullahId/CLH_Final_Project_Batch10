using Dansnom.Contracts;
using Dansnom.Entities.Identity;

namespace Dansnom.Entities
{
    public class Like : AuditableEntity
    {
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}