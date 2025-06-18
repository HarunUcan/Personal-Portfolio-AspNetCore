using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.DashboardDtos;
using PersonalPortfolio.WebUI.Areas.Admin.ViewModels;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");
                }

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:7121/api/VisitorLogs/analytics");

                if (response.IsSuccessStatusCode)
                {
                    var visitorsLogDto = await response.Content.ReadFromJsonAsync<VisitorsLogDto>();
                    var viewModel = new DashboardViewModel { VisitorsLogDto = visitorsLogDto };
                    return View(viewModel);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Veri alınırken bir hata oluştu.");
                }
            }
        }
    }
}
