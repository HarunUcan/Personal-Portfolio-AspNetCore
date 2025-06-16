using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.SkillDtos;
using Newtonsoft.Json;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class SkillController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SkillController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/Skills");
                if (response.IsSuccessStatusCode)
                {
                    var skills = await response.Content.ReadFromJsonAsync<List<ResultSkillDto>>();
                    return View(skills);
                }
                else
                {
                    List<ResultSkillDto> emptyList = new();
                    return View(emptyList);
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSkillDto createSkillDto)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new StringContent(JsonConvert.SerializeObject(createSkillDto), System.Text.Encoding.UTF8, "application/json"))
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync("https://localhost:7121/api/Skills", content);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("Index");
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync($"https://localhost:7121/api/Skills/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var skill = await response.Content.ReadFromJsonAsync<ResultSkillDto>();
                    return View(skill);
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSkillDto updateSkillDto)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new StringContent(JsonConvert.SerializeObject(updateSkillDto), System.Text.Encoding.UTF8, "application/json"))
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsync($"https://localhost:7121/api/Skills/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Skill", new { area = "Admin" });
                }
                else
                {
                    // Handle error response
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");
                
                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.DeleteAsync($"https://localhost:7121/api/Skills/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("Index");
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
