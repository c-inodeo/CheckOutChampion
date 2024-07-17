using CheckOutChampion.Models;
using CheckOutChampion.Models.DTO;
using CheckOutChampion.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CheckOutChampionWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _cartService.LoadCartFromSession(userId, HttpContext.Session) ?? await _cartService.GetCartItems(userId);
            _logger.LogInformation("Cart items loaded from session for user {userId} : {CartItems}", userId, cartItems);
            ViewBag.TotalPrice = await _cartService.GetTotalPrice(userId);
            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItemDto cartItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = new Cart
            { 
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity
            };
            if (!string.IsNullOrEmpty(userId))
            {
                await _cartService.AddOrUpdateCartItem(userId, cartItemDto);
                var cartItems = await _cartService.GetCartItems(userId);
                await _cartService.SaveCartToSession(userId, cartItems, HttpContext.Session);
                _logger.LogInformation("Cart items saved to session for user {userId} : {CartItems}", userId, cartItems);
                TempData["success"] = "Updated cart!";
                ViewBag.TotalPrice = await _cartService.GetTotalPrice(userId);
                return View("Index", cartItems);
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }
        public async Task<IActionResult> RemoveToCart(int cartId)
        {
            await _cartService.RemoveCartItem(cartId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TempData["success"] = "Item deleted successfully!";
            var cartItems = await _cartService.GetCartItems(userId);
            await _cartService.SaveCartToSession(userId, cartItems, HttpContext.Session);
            _logger.LogInformation("Cart items removed and updated in sessionfor user {userId} : {CartItems}", userId, cartItems);
            ViewBag.TotalPrice = await _cartService.GetTotalPrice(userId);
            return View("Index", cartItems);

        }
    }
}
