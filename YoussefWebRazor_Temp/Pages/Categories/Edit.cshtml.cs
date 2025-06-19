using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YoussefWebRazor_Temp.Data;
using YoussefWebRazor_Temp.Models;

namespace YoussefWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _Db;
        [BindProperty]
        public Category category { get; set; }
        public EditModel(ApplicationDbContext Db)
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
            if (ModelState.IsValid)
            {
                _Db.Categories.Update(category);
                _Db.SaveChanges();
                TempData["Success"] = "the category is updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
