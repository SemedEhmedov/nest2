using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            List<Product> products = await context.Products.Include(x=>x.ProductImages).Include(x=>x.TagProducts).ThenInclude(x=>x.Tag).Include(x=>x.Category).ToListAsync();
            return View(products);
        }
    }
}
