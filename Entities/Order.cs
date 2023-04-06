using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Order : AuditableEntity
    {
        public bool isDelivered { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public Sales Sales { get; set; }
        public List<ProductOrders> ProductOrders { get; set; } = new List<ProductOrders>();
    }
}