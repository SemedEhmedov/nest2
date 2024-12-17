using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.DAL;
using Nest1.Models;

namespace Nest1.ViewComponents
{
    public class ShopViewComponent:ViewComponent
    {
        AppDBContext _context;
        public ShopViewComponent(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(string searcht)
        {
            List<Product> products;
            if (searcht != null)
            {
            products = await _context.Products.Include(x=>x.ProductImages).Where(x=>x.Name.ToLower().Contains(searcht.ToLower())).ToListAsync();
            }
            else
            {
                products = await _context.Products.Include(x => x.ProductImages).ToListAsync();
            }



            return View(products);
        }

    }
}
