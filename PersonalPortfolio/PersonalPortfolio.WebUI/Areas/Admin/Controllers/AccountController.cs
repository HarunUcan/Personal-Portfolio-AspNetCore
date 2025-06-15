using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebUI.Areas.Admin.Security;
using System.Security.Claims;
using System.Text.Json;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("admin/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("admin/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre boş olamaz.");
                return View();
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.PostAsJsonAsync("https://localhost:7121/api/Accounts", new { Username = username, Password = password });

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var loginResponse = JsonSerializer.Deserialize<LoginTokenResponse>(responseString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var accessToken = loginResponse?.Token?.AccessToken;

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // 1. accessToken'ı cookie olarak ekle
                        HttpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions
                        {
                            HttpOnly = true,     // JavaScript'ten erişilemesin
                            Secure = true,       // HTTPS zorunlu
                            SameSite = SameSiteMode.Strict,
                            Expires = loginResponse.Token.AccessTokenExpiration
                        });

                        // 2. Claims ve kimlik bilgisi
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, "Admin")
                        };

                        var identity = new ClaimsIdentity(claims, "CookieAuth");
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("CookieAuth", principal);

                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    ModelState.AddModelError("", "Token alınamadı.");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                    return View();
                }
            }
        }
    }
}
