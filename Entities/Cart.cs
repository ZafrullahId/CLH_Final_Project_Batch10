using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dansnom.Contracts;
namespace Dansnom.Entities
{
    public class Cart : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
    }
}