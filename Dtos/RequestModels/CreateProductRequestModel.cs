using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos.RequestModel
{
    public class CreateProductRequestModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
        // public bool isAvailable { get; set; }
    }
}