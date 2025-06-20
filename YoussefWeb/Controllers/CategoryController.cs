using Microsoft.AspNetCore.Mvc;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;

namespace YoussefWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepo;
        public CategoryController(ICategoryRepository CategoryRepo)
        {
            _CategoryRepo = CategoryRepo;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _CategoryRepo.GetAll().ToList();
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
                _CategoryRepo.Add(obj);
                _CategoryRepo.save();
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
            Category? categoryfromDb = _CategoryRepo.Get(u => u.CategoryID == Id);
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
                _CategoryRepo.update(obj);
                _CategoryRepo.save();
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
            Category? categoryfromDb = _CategoryRepo.Get(u => u.CategoryID == Id);
            if (categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            Category? obj = _CategoryRepo.Get(u=>u.CategoryID==Id);
            if (obj == null)
            {
                return NotFound();
            }
            _CategoryRepo.Remove(obj);
            _CategoryRepo.save();
            TempData["Success"] = "the category is deleted successfully";
            return RedirectToAction("Index");
        }

    }
}









//if (obj.Name.ToLower() == "test")
//{
// ModelState.AddModelError("Name", "Test is invalid Value");
//}
