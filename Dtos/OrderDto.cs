namespace Dansnom.Dtos
{
    public class OrderDto
    {
        public AddressDto AddressDto { get; set; }
        public bool isDelivered { get; set; }
        public decimal QuantityBought { get; set; }
        public CustomerDto CustomerDto { get; set; }
    }
}