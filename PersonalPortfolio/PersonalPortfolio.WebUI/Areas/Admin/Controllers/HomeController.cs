using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.HomeDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            using(var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/HomeFeatures");
                if (response.IsSuccessStatusCode)
                {
                    var homeFeatures = await response.Content.ReadFromJsonAsync<ResultHomeDto>();
                    return View(homeFeatures);
                }
                else
                {
                    ResultHomeDto emptyDto = new();
                    return View(emptyDto);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            string? name,
            string? lastName,
            string? description,
            IFormFile? bgImage)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");
                
                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                content.Add(new StringContent(name ?? ""), "Name");
                content.Add(new StringContent(lastName ?? ""), "LastName");
                content.Add(new StringContent(description ?? ""), "Description");
                if (bgImage != null)
                {
                    content.Add(new StreamContent(bgImage.OpenReadStream()), "BgImage", bgImage.FileName);
                    content.Add(new StringContent(bgImage.ContentType ?? ""), "BgImageContentType");
                }

                var response = await client.PutAsync("https://localhost:7121/api/HomeFeatures", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
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
