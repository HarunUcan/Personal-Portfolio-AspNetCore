using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Controllers
{
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
