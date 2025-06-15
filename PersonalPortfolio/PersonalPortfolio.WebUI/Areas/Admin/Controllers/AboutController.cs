using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.AboutDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class AboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AboutController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/Abouts");

                if (response.IsSuccessStatusCode)
                {
                    var abouts = await response.Content.ReadFromJsonAsync<ResultAboutDto>();
                    return View(abouts);
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            string? title,
            string? desc1,
            DateTime birthday,
            string? websiteUrl,
            string? phone,
            string? location,
            string? degree,
            string? email,
            string? freelance,
            IFormFile? profileImage)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");
                }
                if (token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                // Form alanlarını ekle
                content.Add(new StringContent(title ?? ""), "Title");
                content.Add(new StringContent(desc1 ?? ""), "Description1");
                content.Add(new StringContent(birthday.ToString("o")), "Birthday"); // ISO 8601 formatı
                content.Add(new StringContent(websiteUrl ?? ""), "WebsiteUrl");
                content.Add(new StringContent(phone ?? ""), "Phone");
                content.Add(new StringContent(location ?? ""), "Location");
                content.Add(new StringContent(degree ?? ""), "Degree");
                content.Add(new StringContent(email ?? ""), "Email");
                content.Add(new StringContent(freelance ?? ""), "Freelance");

                // Profil resmi varsa ekle
                if (profileImage != null && profileImage.Length > 0)
                {
                    var streamContent = new StreamContent(profileImage.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(profileImage.ContentType);
                    content.Add(streamContent, "ProfileImage", profileImage.FileName);
                }

                // PUT isteği gönder
                var response = await client.PutAsync("https://localhost:7121/api/Abouts", content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılıysa yeniden Index'e yönlendir
                    return RedirectToAction("Index");
                }
                else
                {
                    // Hata sayfası veya mesaj gösterilebilir
                    ViewBag.ErrorMessage = "Güncelleme sırasında bir hata oluştu.";
                    return View("Error");
                }
            }

        }
    }
}
