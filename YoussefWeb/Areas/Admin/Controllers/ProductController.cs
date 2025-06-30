using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Models.ViewModels;
using Youssef.Utility;

namespace YoussefWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webhostenvironment; 
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitOfWork;
            _webhostenvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();
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
                string wwwRootPath = _webhostenvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        //delete the old image
                        var OldImagePath
                            = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('\\'));

                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageURL = @"\images\product\" + filename;

                }

                if (productVM.Product.ProductID == 0)
                {
                    _unitofwork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitofwork.Product.update(productVM.Product);
                }

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

        #region API CALLS


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitofwork.Product.Get(u => u.ProductID == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (!string.IsNullOrEmpty(productToBeDeleted.ImageURL))
            {

                var OldImagePath
                = Path.Combine(_webhostenvironment.WebRootPath, productToBeDeleted.ImageURL.TrimStart('\\'));

                if (System.IO.File.Exists(OldImagePath))
                {
                    System.IO.File.Delete(OldImagePath);
                }

            }

            _unitofwork.Product.Remove(productToBeDeleted);
            _unitofwork.save();

            return Json(new { success = true, message = "Delete Successful" });
        }
            #endregion

    }
}
