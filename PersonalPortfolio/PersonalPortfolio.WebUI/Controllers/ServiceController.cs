﻿using Microsoft.AspNetCore.Mvc;

namespace PersonalPortfolio.WebUI.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
