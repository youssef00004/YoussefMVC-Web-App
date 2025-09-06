using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;

namespace YoussefWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webhostenvironment;
        public OrderController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitOfWork;
            _webhostenvironment = webHostEnvironment;
        }




        public IActionResult Index()
        {
            return View();
        }





        #region API CALLS


        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> objOrderheaders = _unitofwork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = objOrderheaders });
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
