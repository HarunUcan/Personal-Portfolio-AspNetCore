using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly string _tempUserName = "admin";
        private readonly string _tempPassword = "admin123";

        [HttpGet]
        public IActionResult Login()
        {
            // This endpoint is for user registration
            return Ok("User login endpoint");
        }
    }
}
