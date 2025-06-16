using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.User.Dtos.AboutDtos;
using PersonalPortfolio.WebUI.Areas.User.Dtos.SkillDtos;
using PersonalPortfolio.WebUI.Areas.User.ViewModels;

namespace PersonalPortfolio.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class AboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AboutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                AboutPageViewModel aboutPageViewModel;

                var aboutResponse = await client.GetAsync("https://localhost:7121/api/Abouts");
                var skillResponse = await client.GetAsync("https://localhost:7121/api/Skills");

                if (aboutResponse.IsSuccessStatusCode && skillResponse.IsSuccessStatusCode)
                {
                    var about = await aboutResponse.Content.ReadFromJsonAsync<ResultAboutDto>();
                    var skills = await skillResponse.Content.ReadFromJsonAsync<List<ResultSkillDto>>();

                    aboutPageViewModel = new AboutPageViewModel
                    {
                        ResultAboutDto = about,
                        ResultSkillsDto = skills
                    };
                    return View(aboutPageViewModel);
                }
                else
                {
                    return View(aboutPageViewModel = new());
                }
            }
        }
    }
}
