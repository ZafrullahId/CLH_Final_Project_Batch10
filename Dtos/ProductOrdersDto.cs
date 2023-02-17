namespace Dansnom.Dtos
{
    public class ProductOrdersDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool isAvailable { get; set; }
        public AddressDto AddressDto { get; set; }
        public CustomerDto CustomerDto { get; set; }
    }
}