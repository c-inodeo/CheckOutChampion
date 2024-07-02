using CheckOutChampion.Models;
using CheckOutChampion.Services.Interface;
using CheckOutChampion.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public void AddOrUpdateCartItem(string userId, int productId, int quantity, bool isIncrement)
        {
            var cartItem = _unitOfWork.Cart.Get(c => c.UserId == userId && c.ProductId == productId, includeProperties: "Product");

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Product = _unitOfWork.Product.Get(c => c.Id == productId),
                    Quantity = quantity,
                    DateAdded = DateTime.Now
                };
                _unitOfWork.Cart.AddToCart(cartItem);
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
            }
            _unitOfWork.Save();
        }

        public List<Cart> GetCartItems(string userId)
        {
            return _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
        }

        public double GetTotalPrice(string userId)
        {
            var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
            return cartItems.Sum(item => item.Quantity * item.Product.Price);
        }

        public void RemoveCartItem(int cartId)
        {
            var itemToBeDeleted = _unitOfWork.Cart.Get(c => c.CartId == cartId);
            if (itemToBeDeleted != null) 
            { 
                _unitOfWork.Cart.Remove(itemToBeDeleted);
                _unitOfWork.Save();
            }
        }
    }
}
