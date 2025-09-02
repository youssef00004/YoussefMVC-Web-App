using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Models.ViewModels;
using Youssef.Utility;

namespace YoussefWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

             ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserID == UserID, includeProperties: "Product"),
                OrderHeader = new()
             };

            foreach(var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.price * shoppingCart.count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserID == UserID, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == UserID);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.price * shoppingCart.count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserID == UserID, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == UserID);
            ShoppingCartVM.OrderHeader.ApplicationUserId = UserID;

            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.price * shoppingCart.count);
            }

            if(applicationUser.companyID.GetValueOrDefault() == 0)
            {
                // user is a regular customer

                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                // user is a company

                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.save();

            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = shoppingCart.ProductID,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = shoppingCart.price,
                    Count = shoppingCart.count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.save();
            }

            if (applicationUser.companyID.GetValueOrDefault() == 0)
            {
                // user is a regular customer and we need to pay now
                //stripe settings

            }
            else
            {

            }

             return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        public IActionResult Plus (int shoppingcartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartID == shoppingcartId);
            shoppingCart.count += 1;
            _unitOfWork.ShoppingCart.update(shoppingCart);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Minus(int shoppingcartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartID == shoppingcartId);
            if(shoppingCart.count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
            }
            else
            {
                shoppingCart.count -= 1;
                _unitOfWork.ShoppingCart.update(shoppingCart);
            }
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Remove(int shoppingcartId)
        {
            ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartID == shoppingcartId);
            _unitOfWork.ShoppingCart.Remove(shoppingCart);
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));

        }



        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

    }
}
