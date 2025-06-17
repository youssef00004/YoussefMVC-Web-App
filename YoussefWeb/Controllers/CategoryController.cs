using Microsoft.AspNetCore.Mvc;

namespace YoussefWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
