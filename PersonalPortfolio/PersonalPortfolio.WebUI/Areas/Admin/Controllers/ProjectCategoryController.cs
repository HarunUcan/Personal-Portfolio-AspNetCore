using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class ProjectCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectCategoryController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using(var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/ProjectCategories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<ResultProjectCategoryDto>>();
                    return View(categories);
                }
                else
                {
                    List<ResultProjectCategoryDto> emptyList = new List<ResultProjectCategoryDto>();
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
        public async Task<IActionResult> Create(CreateProjectCategoryDto createProjectCategoryDto)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new StringContent(JsonConvert.SerializeObject(createProjectCategoryDto), System.Text.Encoding.UTF8, "application/json"))
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync("https://localhost:7121/api/ProjectCategories", content);
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
            using(var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync($"https://localhost:7121/api/ProjectCategories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var category = await response.Content.ReadFromJsonAsync<ResultProjectCategoryDto>();
                    return View(category);
                }
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProjectCategoryDto updateProjectCategoryDto)
        {
            using(var client = _httpClientFactory.CreateClient())
            using(var content = new StringContent(JsonConvert.SerializeObject(updateProjectCategoryDto), System.Text.Encoding.UTF8, "application/json"))
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                if(token != null)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsync($"https://localhost:7121/api/ProjectCategories/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "ProjectCategory", new { area = "Admin" });
                }
                return View("Error");
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

                var response = await client.DeleteAsync($"https://localhost:7121/api/ProjectCategories/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "ProjectCategory", new { area = "Admin" });
                }
                return View("Error");
            }
        }
    }
}


