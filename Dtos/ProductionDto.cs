using System;
using System.Collections.Generic;
using Dansnom.Enums;

namespace Dansnom.Dtos
{
    public class ProductionDto
    {
        public int ProductionId { get; set; }
        public ProductDto ProductDto { get; set; }
        public List<RawMaterialDto> RawMaterialDto { get; set; }
        public string ProductionDate { get; set; }
        public decimal QuantityProduced { get; set; }
        public decimal QuantityRemaining { get; set; }
        public double QuantityRequest { get; set; }
        public string AdditionalMessage { get; set; }
        public string ApprovalStatus { get; set; }
        public UserDto Admin { get; set; }
        public string PostedTime { get; set; }
        public string CreatedTime { get; set; }
    }
}