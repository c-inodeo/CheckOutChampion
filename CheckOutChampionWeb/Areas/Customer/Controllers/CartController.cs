using CheckOutChampion.DataAccess.Repository;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
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
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetAllCartsByUserId(userId);
            return View(cartItems);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!String.IsNullOrEmpty(userId))
            {
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    DateAdded = DateTime.Now
                };

                _unitOfWork.Cart.AddToCart(cartItem);
                return RedirectToAction("Index");
            }
            else
            {
                //To add 
                return View();
            }
            
        }
    }
}
