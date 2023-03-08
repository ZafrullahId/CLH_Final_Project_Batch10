namespace Dansnom.Dtos
{
    public class ChatDto
    {
        public int SenderId { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public string PostedTime { get; set; }
        public string profileImage { get; set; }
    }
}