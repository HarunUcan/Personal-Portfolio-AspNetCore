using PersonalPortfolio.WebUI.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebUI.Dtos.ProjectImageDtos;

namespace PersonalPortfolio.WebUI.Dtos.PortfolioDtos
{
    public class ResultPortfolioDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<ProjectImageDto>? ProjectImages { get; set; }
        public string? ProjectUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        //public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
        public int? ProjectCategoryId { get; set; }
        public ProjectCategoryDto? ProjectCategory { get; set; }
    }
}
