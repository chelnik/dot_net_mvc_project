using IT_Lab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IT_Lab.Controllers.HomeControllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}