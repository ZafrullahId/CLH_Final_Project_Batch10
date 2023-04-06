using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos.RequestModel
{
    public class UpdateProductRequestModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}