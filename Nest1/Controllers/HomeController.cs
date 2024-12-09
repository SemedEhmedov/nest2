using Microsoft.AspNetCore.Mvc;
using Nest1.DAL;
using Nest1.Models;
using System.Diagnostics;

namespace Nest1.Controllers
{
    public class HomeController : Controller
    {
        AppDBContext context;
        public HomeController(AppDBContext appDBContext)
        {
            context = appDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
