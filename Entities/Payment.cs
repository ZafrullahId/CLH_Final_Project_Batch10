using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class PaymentReference : AuditableEntity
    {
        public string ReferenceNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}