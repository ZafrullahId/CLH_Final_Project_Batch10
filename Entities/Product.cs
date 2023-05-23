using System;
using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool isAvailable { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<ProductOrders> ProductOrders { get; set; } = new List<ProductOrders>();
        public List<Production> Production { get; set; } = new List<Production>();
        public List<Cart> Carts { get; set; }
    }
}