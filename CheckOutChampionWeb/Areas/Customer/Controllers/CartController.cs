using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CheckOutChampionWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
            return View(cartItems);
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = _unitOfWork.Cart.Get(c => c.UserId == userId && c.ProductId == productId, includeProperties: "Product");

            if (!string.IsNullOrEmpty(userId))
            {
                if (cartItem == null)
                {
                    cartItem = new Cart
                    {
                        UserId = userId,
                        ProductId = productId,
                        Product = _unitOfWork.Product.Get(u => u.Id == productId),
                        Quantity = quantity,
                        DateAdded = DateTime.Now
                    };
                    _unitOfWork.Cart.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += quantity;
                    _unitOfWork.Cart.Update(cartItem);
                }

                _unitOfWork.Save();

                // Get the updated list of cart items for the current user
                var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
                return View("AddToCart", cartItems);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
        }
    }
}
