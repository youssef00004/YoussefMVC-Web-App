using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Models.ViewModels;

namespace YoussefWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.GetAll().ToList()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryID.ToString()
                }),

                Product = new Product()
            };
            if (Id == null || Id == 0)
            {
                //create product
                return View(productVM);
            }
            else 
            {
                //update product
                productVM.Product = _unitofwork.Product.Get(u => u.ProductID == Id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM , IFormFile? file )
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Product.Add(productVM.Product);
                _unitofwork.save();
                TempData["Success"] = "the product is added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitofwork.Category.GetAll().ToList()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryID.ToString()
                    });
                return View(productVM);
            }
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Product? ProductfromDb = _unitofwork.Product.Get(u => u.ProductID == Id);
            if (ProductfromDb == null)
            {
                return NotFound();
            }
            return View(ProductfromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? Id)
        {
            Product? obj = _unitofwork.Product.Get(u => u.ProductID == Id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.Product.Remove(obj);
            _unitofwork.save();
            TempData["Success"] = "the category is deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
