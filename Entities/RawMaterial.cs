using System;
using System.Collections.Generic;
using Dansnom.Contracts;
using Dansnom.Enums;

namespace Dansnom.Entities
{
    public class RawMaterial : AuditableEntity
    {
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public decimal Cost { get; set; }
        public double QuantiityBought { get; set; }
        public double QuantiityRemaining { get; set; }
        public string RequestTime { get; set; }
        public string Name { get; set; }
        public string AdditionalMessage { get; set; }
        public List<ProductionRawMaterial> ProductionRawMaterial { get; set; } = new List<ProductionRawMaterial>();
    }
}