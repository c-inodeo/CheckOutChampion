using CheckOutChampion.Models;
using CheckOutChampion.Services.Interface;
using CheckOutChampion.DataAccess.Repository.IRepository;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CheckOutChampion.Models.DTO;

namespace CheckOutChampion.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task AddOrUpdateCartItem(string userId, CartItemDto cartItemDto)
        {
            
            var cartItem = await _unitOfWork.Cart.Get(c => c.UserId == userId && c.ProductId == cartItemDto.ProductId, includeProperties: "Product");

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = cartItemDto.ProductId,
                    Product = await _unitOfWork.Product.Get(c => c.Id == cartItemDto.ProductId),
                    Quantity = cartItemDto.Quantity,
                    DateAdded = DateTime.Now
                };
                await _unitOfWork.Cart.AddToCart(cartItem);
            }
            else
            {
                if (cartItemDto.IsIncrement)
                {
                    cartItem.Quantity += cartItemDto.Quantity;
                }
                else
                {
                    cartItem.Quantity = cartItemDto.Quantity;
                }
            }
            _unitOfWork.Save();
        }

        public async Task<List<Cart>> GetCartItems(string userId)
        {
            var cartItems = await _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product");
            return cartItems.ToList();
        }

        public async Task<double> GetTotalPrice(string userId)
        {
            var cartItems = await _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product");
            return cartItems.ToList().Sum(item => item.Quantity * item.Product.Price);
        }

        public async Task RemoveCartItem(int cartId)
        {
            var itemToBeDeleted = await _unitOfWork.Cart.Get(c => c.CartId == cartId);
            if (itemToBeDeleted != null) 
            { 
                await _unitOfWork.Cart.Remove(itemToBeDeleted);
                _unitOfWork.Save();
            }
        }
        public async Task SaveCartToSession(string userId, List<Cart> cartItems, ISession session)
        {
            session.SetString($"Cart_{userId}", JsonSerializer.Serialize(cartItems)); //CHECK THIS
        }
        public async Task<List<Cart>> LoadCartFromSession(string userId, ISession session)
        {
            var value = session.GetString(userId);
            return value == null ? default(List<Cart>) : JsonSerializer.Deserialize<List<Cart>>(value);
        }

    }
}
