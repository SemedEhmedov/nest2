using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.Areas.ViewModels;
using Nest1.DAL;
using Nest1.Models;

namespace Nest1.Areas.Manage.Controllers
{
    public class CategoryController : ManageController
    {
        AppDBContext _context;
        public CategoryController(AppDBContext dBContext)
        {
            _context = dBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateCategory cg)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            Category category = new Category()
            {
                Name = cg.Name
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return View();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id);


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null)
            {
                return View();
            }
            var category = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(UpdateCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var oldcategory = _context.Categories.FirstOrDefault(s => s.Id == category.Id);
            if (oldcategory == null)
            {
                return NotFound();
            }
            oldcategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
