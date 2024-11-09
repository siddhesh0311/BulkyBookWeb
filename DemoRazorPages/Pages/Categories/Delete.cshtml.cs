using DemoRazorPages.Data;
using DemoRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoRazorPages.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext Db)
        {
            _db = Db;
        }
        public void OnGet(int? id)
        {
            if (id != null)
            {
                category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Category? cat = _db.Categories.Find(category.Id);
                if (cat != null)
                {
                    return NotFound();
                }

                _db.Categories.Remove(category);
                _db.SaveChanges();
                TempData["Success"] = "Category Deleted Successfully!!!";

                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }

        }
    }
}
