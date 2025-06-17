using PersonalPortfolio.WebUI.Areas.User.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebUI.Areas.User.Dtos.ProjectImageDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos
{
    public class CreatePortfolioDto
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
