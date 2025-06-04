namespace PersonalPortfolio.WebApi.Entities
{
    public class ProjectImage
    {
        public int ProjectImageId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
