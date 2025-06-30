using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Utility;

namespace YoussefWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitofwork.Category.GetAll().ToList();
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
                _unitofwork.Category.Add(obj);
                _unitofwork.save();
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
            Category? categoryfromDb = _unitofwork.Category.Get(u => u.CategoryID == Id);
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
                _unitofwork.Category.update(obj);
                _unitofwork.save();
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
            Category? categoryfromDb = _unitofwork.Category.Get(u => u.CategoryID == Id);
            if (categoryfromDb == null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            Category? obj = _unitofwork.Category.Get(u=>u.CategoryID==Id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.Category.Remove(obj);
            _unitofwork.save();
            TempData["Success"] = "the category is deleted successfully";
            return RedirectToAction("Index");
        }

    }
}









//if (obj.Name.ToLower() == "test")
//{
// ModelState.AddModelError("Name", "Test is invalid Value");
//}
