using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.DAL;
using Nest1.Models;

namespace Nest1.ViewComponents
{
    public class ProductViewComponent:ViewComponent
    {
        AppDBContext _context;
        public ProductViewComponent(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(string key)
        {
            List<Product> products = new List<Product>();
            switch (key.ToLower()) 
            {
                case "recentlyadded":products= await _context.Products.OrderByDescending(x => x.Id).Take(4).ToListAsync();
                    break;
                case "topselling":
                    products = await _context.Products.OrderBy(x => x.Count).Take(4).ToListAsync();
                    break;
                case "trending": products = await _context.Products.OrderBy(x => x.Count).Take(4).ToListAsync();
                    break;
                case "nnn":products = await _context.Products.Take(8).ToListAsync();
                    break;
                    default:
                    break;
            }

            return View(products);
        }
    }
}
