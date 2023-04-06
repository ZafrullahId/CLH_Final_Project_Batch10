using System.Collections.Generic;

namespace Dansnom.Dtos.RequestModel
{
    public class CreateOrderRequestModel
    {
         public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? PostalCode { get; set; }
        public string AdditionalDetails { get; set; }
        public List<OrderRequestModel> request { get; set; }
    }   
}