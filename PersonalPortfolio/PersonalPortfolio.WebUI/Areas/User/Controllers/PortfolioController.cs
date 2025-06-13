using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.User.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.User.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebUI.Areas.User.ViewModels;

namespace PersonalPortfolio.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class PortfolioController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PortfolioController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/Projects");
                var responseCategory = await client.GetAsync("https://localhost:7121/api/ProjectCategories");
                if (response.IsSuccessStatusCode && responseCategory.IsSuccessStatusCode)
                {
                    var projects = await response.Content.ReadFromJsonAsync<List<ResultPortfolioDto>>();
                    var categories = await responseCategory.Content.ReadFromJsonAsync<List<ProjectCategoryDto>>();
                    var viewModel = new PortfolioPageViewModel
                    {
                        PortfolioList = projects,
                        ProjectCategoryList = categories
                    };
                    return View(viewModel);
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync($"https://localhost:7121/api/Projects/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var project = await response.Content.ReadFromJsonAsync<ResultPortfolioDto>();
                    if (project == null)
                    {
                        return NotFound();
                    }
                    return View(project);
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }
    }
}
