namespace PersonalPortfolio.WebApi.Dtos.ProjectCategoryDtos
{
    public class ProjectCategoryDto
    {
        public int ProjectCategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
