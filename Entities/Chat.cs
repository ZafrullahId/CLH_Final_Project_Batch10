using System.ComponentModel.DataAnnotations.Schema;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Chat : AuditableEntity
    {
        public string Message { get; set; }
        public bool Seen { get; set; } = false;
        public string PostedTime { get; set; }
        public int SenderId { get; set; }
        public Admin Sender { get; set; }
        public int ReceiverId { get; set; }
    }
}