using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebUI.Areas.Admin.ViewModels;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class PortfolioController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PortfolioController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/Projects");
                if (response.IsSuccessStatusCode)
                {
                    var projects = await response.Content.ReadFromJsonAsync<List<ResultPortfolioDto>>();
                    return View(projects);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync("https://localhost:7121/api/ProjectCategories");
                if (response.IsSuccessStatusCode)
                {
                    PortfolioCreatePageViewModel portfolioCreatePageViewModel = new PortfolioCreatePageViewModel();
                    var categories = await response.Content.ReadFromJsonAsync<List<ResultProjectCategoryDto>>();
                    portfolioCreatePageViewModel.ResultProjectCategories = categories;
                    return View(portfolioCreatePageViewModel);
                }
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PortfolioCreatePageViewModel portfolioCreatePageViewModel)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Form alanlarını ekle
                content.Add(new StringContent(portfolioCreatePageViewModel.CreatePortfolioDto.Name ?? ""), "Name");
                content.Add(new StringContent(portfolioCreatePageViewModel.CreatePortfolioDto.Description ?? ""), "Description");
                content.Add(new StringContent(portfolioCreatePageViewModel.CreatePortfolioDto.ProjectUrl ?? ""), "ProjectUrl");
                content.Add(new StringContent(portfolioCreatePageViewModel.CreatePortfolioDto.ProjectCategoryId.ToString()), "ProjectCategoryId");

                // Profil resmi varsa ekle
                if (portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages != null && portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages.Count > 0)
                {
                    foreach (var image in portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages)
                    {
                        var streamContent = new StreamContent(image.OpenReadStream());
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                        content.Add(streamContent, "ProjectImages", image.FileName);
                    }
                }

                var response = await client.PostAsync("https://localhost:7121/api/Projects", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Portfolio", new { area = "Admin" });
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync($"https://localhost:7121/api/Projects/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var project = await response.Content.ReadFromJsonAsync<ResultPortfolioDto>();
                    if (project == null)
                        return NotFound();

                    PortfolioUpdatePageViewModel portfolioUpdatePageViewModel = new PortfolioUpdatePageViewModel
                    {
                        ResultPortfolioDto = project
                    };

                    var categoryResponse = await client.GetAsync("https://localhost:7121/api/ProjectCategories");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categories = await categoryResponse.Content.ReadFromJsonAsync<List<ResultProjectCategoryDto>>();
                        portfolioUpdatePageViewModel.ResultProjectCategories = categories;
                    }

                    return View(portfolioUpdatePageViewModel);
                }
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PortfolioUpdatePageViewModel portfolioUpdatePageViewModel)
        {
            using (var client = _httpClientFactory.CreateClient())
            using (var content = new MultipartFormDataContent())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Form alanlarını ekle
                content.Add(new StringContent(portfolioUpdatePageViewModel.UpdatePortfolioDto.Name ?? ""), "Name");
                content.Add(new StringContent(portfolioUpdatePageViewModel.UpdatePortfolioDto.Description ?? ""), "Description");
                content.Add(new StringContent(portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectUrl ?? ""), "ProjectUrl");
                content.Add(new StringContent(portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectCategoryId.ToString()), "ProjectCategoryId");

                // Profil resmi varsa ekle
                if (portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages != null && portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages.Count > 0)
                {
                    foreach (var image in portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages)
                    {
                        var streamContent = new StreamContent(image.OpenReadStream());
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                        content.Add(streamContent, "ProjectImages", image.FileName);
                    }
                }

                var response = await client.PutAsync($"https://localhost:7121/api/Projects/{id}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Portfolio", new { area = "Admin" });
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

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync($"https://localhost:7121/api/Projects/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Portfolio", new { area = "Admin" });
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int projectId, int imageId)
        {
            // https://localhost:7121/api/Projects/DeleteImage/{imageId}
            using (var client = _httpClientFactory.CreateClient())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync($"https://localhost:7121/api/Projects/DeleteImage/{imageId}");

                return RedirectToAction("Update", "Portfolio", new { area = "Admin", id = projectId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SetMainImage(int projectId, int imageId)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token Boş veya Geçersiz. Lütfen Giriş Yapın.");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PutAsync($"https://localhost:7121/api/Projects/SetMainImage/{imageId}", null);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Update", "Portfolio", new { area = "Admin", id = projectId });
                return View("Error");
            }
        }
    }
}
