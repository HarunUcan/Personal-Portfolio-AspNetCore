using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Dtos.AccountDtos;
using PersonalPortfolio.WebApi.Security;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly string _tempUserName = "admin";
        private readonly string _tempPassword = "admin123";

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login data cannot be null.");
            }

            // Simulate a user login check
            if (loginDto.Username == _tempUserName && loginDto.Password == _tempPassword)
            {
                Token token = TokenHandler.CreateToken(_configuration);

                // Access token'ı HttpOnly cookie olarak ayarla
                Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Sadece HTTPS isteklerinde gönderilsin
                    SameSite = SameSiteMode.Strict,
                    Expires = token.AccessTokenExpiration
                });

                // Normally, you would generate a JWT token here
                return Ok(new { Message = "Login successful", Token = token });
            }
            else
            {
                return Unauthorized("Invalid username or password.");
            }
        }
    }
}
