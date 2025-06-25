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


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
            #endregion

        }
}
