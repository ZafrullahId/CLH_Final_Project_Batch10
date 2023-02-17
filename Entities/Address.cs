using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Address : AuditableEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? PostalCode { get; set; }
    }
}