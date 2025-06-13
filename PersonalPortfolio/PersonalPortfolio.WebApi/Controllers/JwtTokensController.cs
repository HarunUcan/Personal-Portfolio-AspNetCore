using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Security;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public JwtTokensController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetToken()
        {
            // This endpoint is for generating JWT tokens
            Token token = TokenHandler.CreateToken(_configuration);
            return Ok(token);
        }
    }
}
