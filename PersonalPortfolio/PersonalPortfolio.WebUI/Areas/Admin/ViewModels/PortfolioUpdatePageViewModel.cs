using PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.ViewModels
{
    public class PortfolioUpdatePageViewModel
    {
        public List<ResultProjectCategoryDto>? ResultProjectCategories { get; set; }
        public UpdatePortfolioDto? UpdatePortfolioDto { get; set; }
        public ResultPortfolioDto? ResultPortfolioDto { get; set; }
    }
}
