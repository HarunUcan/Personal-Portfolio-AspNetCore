using PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.ViewModels
{
    public class PortfolioCreatePageViewModel
    {
        public List<ResultProjectCategoryDto>? ResultProjectCategories { get; set; }
        public CreatePortfolioDto? CreatePortfolioDto { get; set; }
    }
}
