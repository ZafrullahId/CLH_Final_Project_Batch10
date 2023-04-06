using System;
using System.Collections.Generic;

namespace Dansnom.Dtos
{
    public class SalesDto
    {
        public decimal AmountPaid { get; set; }
        // public ProductDto ProductDto { get; set; }
        public CustomerDto CustomerDto { get; set; }
        // public string OrderedDate { get; set; }
        // public string DeliveredDate { get; set; }
        public int AddressId { get; set; }
        public List<OrderDto> OrderDtos { get; set; }
    }
}