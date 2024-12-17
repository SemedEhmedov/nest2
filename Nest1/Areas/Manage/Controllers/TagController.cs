using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest1.Areas.ViewModels;
using Nest1.DAL;
using Nest1.Helpers.Extensions;
using Nest1.Models;

namespace Nest1.Areas.Manage.Controllers
{
    public class TagController : ManageController
    {
        AppDBContext _context;
        public TagController(AppDBContext dBContext)
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
        public IActionResult Create(CreateTag ct)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            Tag tag = new Tag()
            {
                Name = ct.Name
            };
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return View();
            }

            Tag tag = await _context.Tags.FirstOrDefaultAsync(x=>x.Id==id);


            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null)
            {
                return View();
            }
            var tag = _context.Tags.FirstOrDefault(s => s.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        public IActionResult Update(UpdateTag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var oldtag = _context.Tags.FirstOrDefault(s => s.Id == tag.Id);
            if (oldtag == null)
            {
                return NotFound();
            }
            oldtag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
