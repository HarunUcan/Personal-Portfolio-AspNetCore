using PersonalPortfolio.WebUI.Areas.User.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.User.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.Areas.User.ViewModels
{
    public class PortfolioPageViewModel
    {
        public List<ResultPortfolioDto> PortfolioList;
        public List<ProjectCategoryDto>? ProjectCategoryList;
    }
}
