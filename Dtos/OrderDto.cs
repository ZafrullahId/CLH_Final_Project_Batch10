namespace Dansnom.Dtos
{
    public class OrderDto
    {
        // public AddressDto AddressDto { get; set; }
        // public bool isDelivered { get; set; }
        public ProductDto ProductDto { get; set; }
        public decimal QuantityBought { get; set; }
        public string OrderedDate { get; set; }
        public string DeleveredDate { get; set; }
        public decimal AmountPaid { get; set; }

        // public CustomerDto CustomerDto { get; set; }
    }
}