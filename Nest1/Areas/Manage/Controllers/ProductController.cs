using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.Areas.ViewModels;
using Nest1.DAL;
using Nest1.Helpers.Extensions;
using Nest1.Models;

namespace Nest1.Areas.Manage.Controllers
{
    public class ProductController : ManageController
    {
        AppDBContext context;
        private readonly IWebHostEnvironment env;

        public ProductController(AppDBContext appDBcontext, IWebHostEnvironment env)
        {
            context = appDBcontext;
            this.env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await context.Products.Include(x => x.Category).Include(x => x.TagProducts).ThenInclude(x => x.Tag).ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProduct vm)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (vm.File == null)
            {
                ModelState.AddModelError("File", "Fayl secilmeyib.");
                return View();
            }
            if (!vm.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File", "fayl formati sehvdir");
                return View();
            }
            if (vm.File.Length > 2097152)
            {
                ModelState.AddModelError("File", "sekilin olcusu 2mb dan cox ola bilmez");
                return View();
            }
            vm.ImgUrl = vm.File.Upload(env.WebRootPath, "Upload\\Product");

            if (!ModelState.IsValid)
            {
                return View();
            }
            Product product = new Product()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                ImgUrl = vm.ImgUrl
            };
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return View();
            }
            var product = context.Products.FirstOrDefault(s => s.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            FileExtension.DeleteFile(env.WebRootPath, "Upload\\Product", product.ImgUrl);
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (id == null)
            {
                return View();
            }
            var product = context.Products.FirstOrDefault(s => s.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(Product product)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var oldproduct = context.Products.FirstOrDefault(s => s.Id == product.Id);
            if (oldproduct == null)
            {
                return NotFound();
            }
            oldproduct.Name = product.Name;
            oldproduct.Description = product.Description;
            oldproduct.Price = product.Price;
            oldproduct.CategoryId = product.CategoryId;
            oldproduct.ImgUrl = product.File.Upload(env.WebRootPath, "Upload\\Product");
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
