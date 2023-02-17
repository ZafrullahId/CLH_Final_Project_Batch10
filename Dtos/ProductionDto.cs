using System;
using Dansnom.Enums;

namespace Dansnom.Dtos
{
    public class ProductionDto
    {
        public ProductDto ProductDto { get; set; }
        public RawMaterialDto RawMaterialDto { get; set; }
        public string ProductionDate { get; set; }
        public decimal QuantityProduced { get; set; }
        public decimal QuantityRemaining { get; set; }
        public double QuantityRequest { get; set; }
        public string AdditionalMessage { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}