using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dansnom.Contracts;
namespace Dansnom.Entities
{
    public class Cart : AuditableEntity
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}