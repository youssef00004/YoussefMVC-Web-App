using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;
using Youssef.Models.ViewModels;

namespace YoussefWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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

            };

            foreach(var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderTotal += (shoppingCart.price * shoppingCart.count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
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
