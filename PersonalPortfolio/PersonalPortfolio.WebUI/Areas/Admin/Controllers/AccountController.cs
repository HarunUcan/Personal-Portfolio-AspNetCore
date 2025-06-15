using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        [Route("admin/login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
