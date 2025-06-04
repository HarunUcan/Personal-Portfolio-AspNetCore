using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
