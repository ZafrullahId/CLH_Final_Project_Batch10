namespace Dansnom.Dtos
{
    public class OrderDto
    {
        public ProductDto ProductDto { get; set; }
        public decimal QuantityBought { get; set; }
        public string OrderedDate { get; set; }
        public string DeleveredDate { get; set; }
        public decimal AmountPaid { get; set; }
    }
}