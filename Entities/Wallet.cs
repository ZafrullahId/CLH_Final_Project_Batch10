using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Wallet : AuditableEntity
    {
        public decimal Total { get; set; }       
    }
}