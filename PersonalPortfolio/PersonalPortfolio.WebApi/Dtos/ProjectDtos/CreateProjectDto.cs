using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Dtos.ProjectDtos
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<string>? ProjectImages { get; set; }
        public string? ProjectUrl { get; set; }

        // Navigation properties
        public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
        public int? ProjectCategoryId { get; set; }
        public ProjectCategory? ProjectCategory { get; set; }
    }
}
