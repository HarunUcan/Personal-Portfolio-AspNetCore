namespace PersonalPortfolio.WebApi.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? MessageDetails { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
