using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.User.Dtos.HomeFeatureDtos;
using PersonalPortfolio.WebUI.Areas.User.Models;
using System.Diagnostics;

namespace PersonalPortfolio.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7121/api/HomeFeatures");
            if (response.IsSuccessStatusCode)
            {
                var homeFeature = await response.Content.ReadFromJsonAsync<ResultHomeFeatureDto>();
                return View(homeFeature);
            }
            else
            {
                ResultHomeFeatureDto emptyDto = new();
                return View(emptyDto);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
