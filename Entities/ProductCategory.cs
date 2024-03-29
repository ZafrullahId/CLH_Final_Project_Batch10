using System;
using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> Product { get; set; }
    }
}