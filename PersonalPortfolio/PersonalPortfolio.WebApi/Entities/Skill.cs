namespace PersonalPortfolio.WebApi.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Percentage { get; set; } // Assuming this is a percentage value for the skill level
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
