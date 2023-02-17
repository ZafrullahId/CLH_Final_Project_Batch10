using System;

namespace Dansnom.Dtos
{
    public class SalesDto
    {
        public decimal QuantityBought { get; set; }
        public decimal AmountPaid { get; set; }
        public ProductDto ProductDto { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public string OrderedDate { get; set; }
        public string DeliveredDate { get; set; }
    }
}