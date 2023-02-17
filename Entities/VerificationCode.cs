using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class VerificationCode : AuditableEntity
    {
        public int Code { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}