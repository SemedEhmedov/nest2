using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.DAL;

namespace Nest1.Controllers
{
    public class ShopController : Controller
    {
        AppDBContext _context;
        public ShopController(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Filter(string search)
        {
            return ViewComponent("Shop", search);
        }
    }
}
