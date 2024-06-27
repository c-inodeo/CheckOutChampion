using CheckOutChampion.DataAccess.Repository;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = _unitOfWork.Cart.Get(c => c.UserId == userId && c.ProductId == productId);

            //ProductVM productVM = new(){ 
            //    Cart = new Cart()
            //};

            if (!String.IsNullOrEmpty(userId))
            {
                if (cartItem == null)
                {
                    cartItem = new Cart
                    {
                        UserId = userId,
                        ProductId = productId,
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
                return View("Index");
            }
            else
            {
                //To add 
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.Get(c => c.UserId == userId, includeProperties: "Product");
            return View(cartItems);
        }

    }
}
