namespace PersonalPortfolio.WebApi.Entities
{
    public class About
    {
        public int AboutId { get; set; }
        public string? Title { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public DateTime Birthday { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? Degree { get; set; }
        public string? Email { get; set; }
        public string? Freelance { get; set; }
    }
}
