namespace PersonalPortfolio.WebApi.Entities
{
    public class ProjectTag
    {
        public int ProjectTagId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        // Additional properties can be added here if needed
        // For example, you might want to track when the tag was added to the project
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
