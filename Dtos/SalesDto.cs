using System;
using System.Collections.Generic;

namespace Dansnom.Dtos
{
    public class SalesDto
    {
        public decimal AmountPaid { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public List<OrderDto> OrderDtos { get; set; }
    }
}