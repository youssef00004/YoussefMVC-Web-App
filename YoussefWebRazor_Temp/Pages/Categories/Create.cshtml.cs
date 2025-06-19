using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoussefWebRazor_Temp.Data;
using YoussefWebRazor_Temp.Models;

namespace YoussefWebRazor_Temp.Pages.Categories
{
    //[BindProperties]  # can be also used
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _Db;
        [BindProperty]
        public Category category { get; set; }
        public CreateModel(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _Db.Categories.Add(category);
            _Db.SaveChanges();
            TempData["success"] = "Category is added successfully";
            return RedirectToPage("Index");
        }
    }
}
