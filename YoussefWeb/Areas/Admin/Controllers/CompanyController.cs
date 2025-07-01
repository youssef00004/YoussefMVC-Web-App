using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Utility;

namespace YoussefWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork; 
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? Id)
        {
 
            if (Id == null || Id == 0)
            {
                //create product
                return View(new Company());
            }
            else 
            {
                //update product
                Company company = _unitofwork.Company.Get(u => u.CompanyId == Id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {

                if (company.CompanyId == 0)
                {
                    _unitofwork.Company.Add(company);
                }
                else
                {
                    _unitofwork.Company.update(company);
                }

                _unitofwork.save();
                TempData["Success"] = "the product is added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(company);
            }
        }

        #region API CALLS


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitofwork.Company.Get(u => u.CompanyId == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitofwork.Company.Remove(companyToBeDeleted);
            _unitofwork.save();

            return Json(new { success = true, message = "Delete Successful" });
        }
            #endregion

    }
}
