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

        public ProductController(AppDBContext appdbcontext, IWebHostEnvironment env)
        {
            context = appdbcontext;
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
            if (!ModelState.IsValid)
            {
                return View();
            }
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Tags = context.Tags.ToList();
            Product product = new Product()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
            };
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
            ProductImage productImage = (new()
            {
                ImgUrl = vm.File.Upload(env.WebRootPath,"Upload\\Product"),
                Primary = true,
                Product = product
            });
            if (vm.Images != null)
            {
                List<ProductImage> ProductImages = new List<ProductImage>();
                foreach (var image in vm.Images)
                {
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
                productImage = (new()
                {
                    ImgUrl = image.Upload(env.WebRootPath,"Upload/Product"),
                    Primary = false,
                    Product = product
                });
                ProductImages.Add(productImage);
                }
                context.ProductImages.AddRange(ProductImages);  
            }
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return View();
            }

            Product product = await context.Products.Include(x=>x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);

            List<ProductImage> productImages = product.ProductImages;
            context.ProductImages.RemoveRange(productImages);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Tags = context.Tags.ToList();

            if (id == null)
            {
                return NotFound();
            }
            var product = await context.Products.Include(x => x.Category)
                .Include(p => p.ProductImages)
                .Include(c => c.TagProducts)
                .ThenInclude(z => z.Tag)
                .FirstOrDefaultAsync(g => g.Id == id);
            ViewBag.Categories = context.Categories.ToList();
            UpdateProduct Vm = new UpdateProduct()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                TagIds = new List<int>(),
                Images = new List<IFormFile>()

            };
            foreach (var item in product.TagProducts)
            {
                Vm.TagIds.Add(item.Id);
            }
            return View(Vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProduct vm)
        {
            if (vm.Id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Product oldProduct = context.Products.Include(p => p.TagProducts).Include(p => p.ProductImages).FirstOrDefault(x => x.Id == vm.Id);
            if (oldProduct == null)
            {
                return NotFound();
            }
            if (vm.CategoryId != null)
            {
                if (!context.Categories.Any(c => c.Id == vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", $"{vm.CategoryId}-idli category yoxdur");
                    return View();
                }
            }


            if (vm.TagIds != null)
            {
                context.TagProducts.RemoveRange(oldProduct.TagProducts);

                foreach (var item in vm.TagIds)
                {
                    await context.TagProducts.AddAsync(new TagProduct()
                    {
                        ProductId = oldProduct.Id,
                        TagId = item
                    });

                }

            }


            if (vm.File != null)
            {
                if (!vm.File.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sekil daxil edin");
                    return View(vm);
                }
                if (vm.File.Length > 3000000)
                {
                    ModelState.AddModelError("MainPhoto", "Max 2mb ola biler");
                    return View(vm);
                }

                FileExtension.DeleteFile(env.WebRootPath, "Upload/Product", oldProduct.ProductImages.FirstOrDefault(x => x.Primary).ImgUrl);
                context.ProductImages.Remove(oldProduct.ProductImages.FirstOrDefault(x => x.Primary));

                oldProduct.ProductImages.Add(new()
                {
                    Primary = true,
                    ImgUrl = vm.File.Upload(env.WebRootPath, "Upload/Product")
                });



            }

            if (vm.Images != null)
            {
                var removeImgs = new List<ProductImage>();
                foreach (var imgUrl in oldProduct.ProductImages.Where(x => x.Primary == false))
                {
                    if (!vm.Images.Any(x => x.ToString() == imgUrl.ImgUrl))
                    {
                        FileExtension.DeleteFile(env.WebRootPath, "Upload/Product", imgUrl.ImgUrl);
                        context.ProductImages.Remove(imgUrl);
                    }

                }
            }
            else
            {
                foreach (var item in oldProduct.ProductImages.Where(x => !x.Primary))
                {
                    FileExtension.DeleteFile(env.WebRootPath, "Upload/Product", item.ImgUrl);
                    context.ProductImages.Remove(item);
                }
            }
            if (vm.Images != null)
            {
                foreach (var image in vm.Images)
                {
                    if (!image.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Photos", "Sekil yoxdur");
                        return View();
                    }
                    if (image.Length > 2000000)
                    {
                        ModelState.AddModelError("Photos", "Max 2mb ola biler");
                        return View();
                    }
                    oldProduct.ProductImages.Add(new()
                    {
                        Primary = false,
                        ImgUrl = image.Upload(env.WebRootPath, "Upload/Product")
                    });
                }
            }
            oldProduct.Name = vm.Name;
            oldProduct.Price = vm.Price;
            oldProduct.Description = vm.Description;
            oldProduct.CategoryId = vm.CategoryId;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
