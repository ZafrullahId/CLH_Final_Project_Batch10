namespace Dansnom.Dtos.RequestModel
{
    public class CreateOrderRequestModel
    {
        public decimal QuantityBought { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? PostalCode { get; set; }
    }   
}