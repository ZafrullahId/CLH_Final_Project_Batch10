using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Dansnom.Dtos
{
    public class LikeDto
    {
        public int numberOfLikes { get; set; }
        public List<CustomerDto> CustomerDto { get; set; } 
    }
}