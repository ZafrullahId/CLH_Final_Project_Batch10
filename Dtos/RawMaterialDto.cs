using System;
using Dansnom.Enums;

namespace Dansnom.Dtos
{
    public class RawMaterialDto
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public double QuantiityBought { get; set; }
        public double QuantiityRemaining { get; set; }
        public string Name { get; set; }
        public string AdditionalMessage { get; set; }
        public string PostedTime { get; set; }
        public ApprovalStatus EnumApprovalStatus { get; set; }
        public string StringApprovalStatus { get; set; }
        public string CreatedTime { get; set; }
    }
}