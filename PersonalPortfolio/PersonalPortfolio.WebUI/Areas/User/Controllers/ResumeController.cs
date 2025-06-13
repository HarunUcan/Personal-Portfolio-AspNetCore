using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
