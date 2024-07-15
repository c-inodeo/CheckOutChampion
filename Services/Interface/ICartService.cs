using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckOutChampion.Models;
using CheckOutChampion.Models.DTO;
using Microsoft.AspNetCore.Http;

namespace CheckOutChampion.Services.Interface
{
    public interface ICartService
    {
        List<Cart> GetCartItems(string userId);
        void AddOrUpdateCartItem(string userId,CartItemDto cartItemDto);
        void RemoveCartItem(int cartId);
        double GetTotalPrice(string userId);
        void SaveCartToSession(string userId, List<Cart> cartItems, ISession session);
        List<Cart> LoadCartFromSession(string userId, ISession session);
    }
}
