using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Review : AuditableEntity
    {
        public string Text { get; set; }
        public bool Seen { get; set; } = false;
        public string PostedTime { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<Like> Likes { get; set; } = new List<Like>();
    }
}