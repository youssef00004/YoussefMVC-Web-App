using Microsoft.AspNetCore.Mvc;
using Youssef.DataAccess.Data;
using Youssef.Models;

namespace YoussefWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _Db;
        public CategoryController(ApplicationDbContext Db)
        {
            _Db = Db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _Db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "the DisplayOrder cannot exactly match the name");
            }
            //if (obj.Name.ToLower() == "test")
            //{
            // ModelState.AddModelError("Name", "Test is invalid Value");
            //}
            if (ModelState.IsValid)
            {
                _Db.Categories.Add(obj);
                _Db.SaveChanges();
                TempData["Success"] = "the category is added successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {
            if(Id == null || Id==0)
            {
                return NotFound();
            }
            Category? categoryfromDb = _Db.Categories.Find(Id);
            //Category? categoryfromDb2 = _Db.Categories.FirstOrDefault(u=>u.CategoryID==Id);
            //Category? categoryfromDb3 = _Db.Categories.Where(u => u.CategoryID == Id).FirstOrDefault();
            if (categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _Db.Categories.Update(obj);
                _Db.SaveChanges();
                TempData["Success"] = "the category is updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category? categoryfromDb = _Db.Categories.Find(Id);
            if (categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            Category? obj = _Db.Categories.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            _Db.Categories.Remove(obj);
            _Db.SaveChanges();
            TempData["Success"] = "the category is deleted successfully";
            return RedirectToAction("Index");
        }

    }
}









//if (obj.Name.ToLower() == "test")
//{
// ModelState.AddModelError("Name", "Test is invalid Value");
//}
