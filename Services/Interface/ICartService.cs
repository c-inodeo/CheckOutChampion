using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckOutChampion.Models;
using Microsoft.AspNetCore.Http;

namespace CheckOutChampion.Services.Interface
{
    public interface ICartService
    {
        List<Cart> GetCartItems(string userId);
        void AddOrUpdateCartItem(string userId, int productId, int quantity, bool isIncrement);
        void RemoveCartItem(int cartId);
        double GetTotalPrice(string userId);
        void SaveCartToSession(string userId, List<Cart> cartItems, ISession session);
        List<Cart> LoadCartFromSession(string userId, ISession session);
    }
}
