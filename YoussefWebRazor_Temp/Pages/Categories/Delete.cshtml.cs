using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoussefWebRazor_Temp.Data;
using YoussefWebRazor_Temp.Models;

namespace YoussefWebRazor_Temp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _Db;
        [BindProperty]
        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public void OnGet(int? Id)
        {
            if (Id != null && Id != 0)
            {
                category = _Db.Categories.Find(Id);
            }

        }

        public IActionResult OnPost()
        {
            category = _Db.Categories.Find(category.CategoryID);
            if (category == null)
            {
                return NotFound();
            }
            _Db.Categories.Remove(category);
            _Db.SaveChanges();
            TempData["Success"] = "the category is deleted successfully";
            return RedirectToPage("Index");
        }
    }
}

