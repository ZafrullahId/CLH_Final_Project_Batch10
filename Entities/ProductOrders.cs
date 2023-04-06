using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class ProductOrders : AuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal QuantityBought { get; set; }
    }
}