using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, bool isIncrement)
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
                    if (isIncrement)
                    {
                        cartItem.Quantity += quantity;
                    }
                    else
                    {
                        cartItem.Quantity = quantity;
                    }

                    _unitOfWork.Cart.Update(cartItem);
                }

                _unitOfWork.Save();

                // Get the updated list of cart items for the current user
                var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
                TempData["success"] = "Updated cart!";
                ViewBag.TotalPrice = GetTotalPrice();

                return View("Index", cartItems);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
        }
        [HttpPost]
        public IActionResult RemoveToCart(int cartId)
        {
            var itemToBeDeleted = _unitOfWork.Cart.Get(u=> u.CartId ==  cartId);

            if (itemToBeDeleted != null)
            {
                _unitOfWork.Cart.Remove(itemToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Item deleted successfully!";
                return RedirectToAction();
            }
            else 
            {
                return NotFound();
            }

        }
        public double GetTotalPrice() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
            double totalPrice = cartItems.Sum(item => item.Quantity * item.Product.Price);
            
            return totalPrice;
        }
    }
}
