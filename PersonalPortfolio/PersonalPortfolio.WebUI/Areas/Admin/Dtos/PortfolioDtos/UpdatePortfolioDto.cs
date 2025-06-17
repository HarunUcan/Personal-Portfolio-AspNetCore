using PersonalPortfolio.WebUI.Areas.User.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos
{
    public class UpdatePortfolioDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<IFormFile>? ProjectImages { get; set; }
        public string? ProjectUrl { get; set; }

        // Navigation properties
        public int? ProjectCategoryId { get; set; }
        public ProjectCategoryDto? ProjectCategory { get; set; }
    }
}
