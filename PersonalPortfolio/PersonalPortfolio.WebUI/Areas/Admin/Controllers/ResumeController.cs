using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ResumeDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class ResumeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResumeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateResumeDto updateResumeDto)
        {
            if (updateResumeDto.PdfFile == null || updateResumeDto.PdfFile.Length == 0)
            {
                return BadRequest("Lütfen bir dosya seçin.");
            }

            using (var client = _httpClientFactory.CreateClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");
                }

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                content.Add(new StreamContent(updateResumeDto.PdfFile.OpenReadStream()), "PdfFile", updateResumeDto.PdfFile.FileName);

                var response = await client.PutAsync("https://localhost:7121/api/Resumes", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Dosya yüklenirken bir hata oluştu.");
                }
            }
        }
    }
}
