using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
