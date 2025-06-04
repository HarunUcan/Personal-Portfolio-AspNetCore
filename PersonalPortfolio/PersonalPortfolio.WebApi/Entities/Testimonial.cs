namespace PersonalPortfolio.WebApi.Entities
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public int NumberOfStars { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
