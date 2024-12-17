using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.DAL;
using Nest1.ViewModels.Basket;
using Newtonsoft.Json;

namespace Nest1.Controllers
{
    public class CartController : Controller
    {
        AppDBContext _context;
        public CartController(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<IActionResult> Index()
        {
            var json = Request.Cookies["basket"];
            List<CookieItemVm> cookies = new List<CookieItemVm>();

            if (json != null)
            {
                cookies= JsonConvert.DeserializeObject<List<CookieItemVm>>(json);
            }
            List<CartVm> cart = new List<CartVm>();
            if (cookies.Count > 0)
            {
                cookies.ForEach(async c =>
                {
                    var product = await _context.Products.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == c.Id);
                    if (product == null)
                    {
                        cookies.Remove(c);
                    }
                    else
                    {
                        cart.Add(new CartVm
                        {
                            Id = c.Id,
                            Name = product.Name,
                            Price = product.Price,
                            ImgUrl=product.ProductImages.FirstOrDefault(x=>x.Primary).ImgUrl,
                            Count = c.Count
                        });
                    }
                });
                Response.Cookies.Append("basket",JsonConvert.SerializeObject(cookies));
            }



            return View(cart);
        }
        public async Task<IActionResult> AddBasket(int itemId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x=>x.Id == itemId);
            if (product == null) return NotFound();
            List<CookieItemVm> cookieItemVms;
            var basket = Request.Cookies["basket"];
            if (basket != null)
            {
                cookieItemVms = JsonConvert.DeserializeObject<List<CookieItemVm>>(basket);
                var exsistproduct = cookieItemVms.FirstOrDefault(x => x.Id == itemId);
                if (exsistproduct != null)
                {
                    exsistproduct.Count += 1;
                }
                else
                {
                    cookieItemVms.Add(new CookieItemVm { Id = itemId,Count=1 });
                }
            }
            else
            {
                cookieItemVms= new List<CookieItemVm>();
                cookieItemVms.Add(new CookieItemVm { Id = itemId, Count = 1 });
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItemVms));








            return RedirectToAction("Index", "Home");
        }
        public IActionResult GetBasket()
        {
            var json = JsonConvert.DeserializeObject<CookieItemVm>(Request.Cookies["basket"]);
            return View();
        } 
    }
}
