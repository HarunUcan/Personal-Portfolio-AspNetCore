using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Dtos.ProjectCategoryDtos
{
    public class CreateProjectCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation properties
        public ICollection<Project>? Projects { get; set; }
    }
}
