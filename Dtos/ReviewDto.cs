using System;

namespace Dansnom.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Text {get;set;}
        public string ImageUrl { get; set; }
        public bool Seen { get; set; }
        public string  PostedTime{ get; set; }
    }
}