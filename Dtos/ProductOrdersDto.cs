using System.Collections.Generic;

namespace Dansnom.Dtos
{
    public class ProductOrdersDto
    {
        public List<OrderDto> OrderDtos { get; set; }
        public AddressDto AddressDto { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public bool isDelivered { get; set; }
        public decimal NetAmount { get; set; }
        public int OrderId { get; set; }
    }
}