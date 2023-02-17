namespace Dansnom.Dtos.RequestModel
{
    public class UpdateRawMaterialRequestModel
    {
        public decimal Cost { get; set; }
        public double QuantiityBought { get; set; }
        public string Name { get; set; }
        public string AdditionalMessage { get; set; }
    }
}