using PersonalPortfolio.WebUI.Dtos.AboutDtos;
using PersonalPortfolio.WebUI.Dtos.SkillDtos;

namespace PersonalPortfolio.WebUI.ViewModels
{
    public class AboutPageViewModel
    {
        public ResultAboutDto ResultAboutDto { get; set; }
        public List<ResultSkillDto> ResultSkillsDto { get; set; }
    }
}
