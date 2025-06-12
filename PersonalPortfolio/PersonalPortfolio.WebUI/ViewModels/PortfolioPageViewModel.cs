using PersonalPortfolio.WebUI.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.ViewModels
{
    public class PortfolioPageViewModel
    {
        public List<ResultPortfolioDto> PortfolioList;
        public List<ProjectCategoryDto>? ProjectCategoryList;
    }
}
