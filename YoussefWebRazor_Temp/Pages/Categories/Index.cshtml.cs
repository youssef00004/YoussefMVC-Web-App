using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoussefWebRazor_Temp.Data;
using YoussefWebRazor_Temp.Models;

namespace YoussefWebRazor_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _Db;
        public List<Category> CategoryList { get; set; }
        public IndexModel(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public void OnGet()
        {
            CategoryList = _Db.Categories.ToList();
        }
    }
}
