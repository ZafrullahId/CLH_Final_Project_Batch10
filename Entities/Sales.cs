using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Sales : AuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal AmountPaid { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}