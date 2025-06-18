using ECommerceProject.BusinessLayer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.PortfolioDtos;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebUI.Areas.Admin.Dtos.ProjectImageDtos;
using PersonalPortfolio.WebUI.Areas.Admin.ViewModels;
using System.IO;
using System.Text.Json;

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
                    foreach(var project in projects)
                    {
                        foreach(var image in project.ProjectImages)
                        {
                            image.Url = image.Url.Replace("wwwroot", "");
                        }
                    }
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


                // Proje resmi varsa ekle
                if (portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages != null && portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages.Count > 0)
                {
                    foreach (var image in portfolioCreatePageViewModel.CreatePortfolioDto.ProjectImages)
                    {
                        string filePath = string.Empty;
                        using (var memoryStream = new MemoryStream())
                        {
                            // Resmi wwwroot/uploads a kaydet
                            await image.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();
                            var imageName = image.FileName;

                            filePath = await FileHelper.SaveFileAsync(imageData, $"{Guid.NewGuid()}{Path.GetExtension(imageName)}");
                        }

                        content.Add(new StringContent(filePath), "ProjectImages");

                        //var streamContent = new StreamContent(image.OpenReadStream());
                        //streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                        //content.Add(streamContent, "ProjectImages", image.FileName);
                    }
                }

                try
                {
                    var response = await client.PostAsync("https://localhost:7121/api/Projects", content);

                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Index", "Portfolio", new { area = "Admin" });

                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Content($"Hata: {response.StatusCode} - {errorMsg}");
                }
                catch (Exception ex)
                {
                    return Content($"İstisna oluştu: {ex.Message}\n\n{ex.StackTrace}");
                }
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

                // Proje resmi varsa ekle
                if (portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages != null && portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages.Count > 0)
                {
                    foreach (var image in portfolioUpdatePageViewModel.UpdatePortfolioDto.ProjectImages)
                    {
                        string filePath = string.Empty;
                        using (var memoryStream = new MemoryStream())
                        {
                            // Resmi wwwroot/uploads a kaydet
                            await image.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();
                            var imageName = image.FileName;

                            filePath = await FileHelper.SaveFileAsync(imageData, $"{Guid.NewGuid()}{Path.GetExtension(imageName)}");

                        }
                        content.Add(new StringContent(filePath), "ProjectImages");

                        //var streamContent = new StreamContent(image.OpenReadStream());
                        //streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                        //content.Add(streamContent, "ProjectImages", image.FileName);
                    }
                }

                try
                {
                    var response = await client.PutAsync($"https://localhost:7121/api/Projects/{id}", content);

                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Index", "Portfolio", new { area = "Admin" });

                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Content($"Hata: {response.StatusCode} - {errorMsg}");
                }
                catch (Exception ex)
                {
                    return Content($"İstisna oluştu: {ex.Message}\n\n{ex.StackTrace}");
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
                if (response.IsSuccessStatusCode)
                {
                    // imageUrl'i JSON'dan al
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(jsonString);
                    var imageUrl = json.RootElement.GetProperty("imageUrl").GetString();

                    // wwwroot/uploads/... formatında gelen yolu olduğu gibi fiziksel tam yola çevir
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        FileHelper.DeleteFile(imageUrl);
                    }
                }

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
