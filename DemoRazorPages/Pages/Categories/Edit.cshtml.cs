using DemoRazorPages.Data;
using DemoRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoRazorPages.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category category { get; set; }

        public EditModel(ApplicationDbContext Db)
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
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["Success"] = "Category Edited Successfully!!!";

                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
