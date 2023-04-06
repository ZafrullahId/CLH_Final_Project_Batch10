using System;
using System.Collections.Generic;

namespace Dansnom.Dtos
{
    public class ProductSaleDto
    {
        public decimal AmountPaid { get; set; }
        public decimal QuantityBought { get; set; }
        public ProductDto ProductDto { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public int AddressId { get; set; }
        public string OrderedDate { get; set; }
        public string DeleveredDate { get; set; }
    }
}