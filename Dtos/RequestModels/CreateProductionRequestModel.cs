using System;

namespace Dansnom.Dtos.RequestModel
{
    public class CreateProductionRequestModel
    {
        public string ProductName { get; set; }
        public double QuantityRequest { get; set; }
        public int QuantityProduced { get; set; }
        public string AdditionalMessage { get; set; }
    }
}