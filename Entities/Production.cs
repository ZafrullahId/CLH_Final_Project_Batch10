using System;
using System.Collections.Generic;
using Dansnom.Contracts;
using Dansnom.Enums;

namespace Dansnom.Entities
 {
      public class Production : AuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ProductionDate { get; set; } = DateTime.Now.ToLongDateString();
        public double QuantityRequest { get; set; }
        public decimal QuantityProduced { get; set; }
        public decimal QuantityRemaining { get; set; }
        public string RequestTime { get; set; }
        public string AdditionalMessage { get; set; }
       public List<ProductionRawMaterial> ProductionRawMaterial { get; set; } = new List<ProductionRawMaterial>();
    }
 }
    