using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Repo.IRepository;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utilities;

namespace AspNet8DemoMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _context;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _context = UnitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.CategoryRepository.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.CategoryRepository.Add(category);
                _context.Save();
                TempData["Success"] = "Category Created Successfully!!!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category? category = _context.CategoryRepository.Get(u => u.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _context.CategoryRepository.Update(obj);
                _context.Save();
                TempData["Success"] = "Category Updated Successfully!!!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category? category = _context.CategoryRepository.Get(u => u.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(Category obj)
        {
            if (ModelState.IsValid)
            {
                Category? category = _context.CategoryRepository.Get(u => u.Id == obj.Id);
                if (category == null)
                {
                    return NotFound();
                }
                _context.CategoryRepository.Remove(category);
                _context.Save();
                TempData["Success"] = "Category Deleted Successfully!!!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
    }
}
