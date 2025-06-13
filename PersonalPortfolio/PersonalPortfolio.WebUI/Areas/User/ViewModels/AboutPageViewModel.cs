using PersonalPortfolio.WebUI.Areas.User.Dtos.AboutDtos;
using PersonalPortfolio.WebUI.Areas.User.Dtos.SkillDtos;

namespace PersonalPortfolio.WebUI.Areas.User.ViewModels
{
    public class AboutPageViewModel
    {
        public ResultAboutDto ResultAboutDto { get; set; }
        public List<ResultSkillDto> ResultSkillsDto { get; set; }
    }
}
