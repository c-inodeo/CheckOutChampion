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
        Task<List<Cart>> GetCartItems(string userId);
        Task AddOrUpdateCartItem(string userId,CartItemDto cartItemDto);
        Task RemoveCartItem(int cartId);
        Task<double> GetTotalPrice(string userId);
        Task SaveCartToSession(string userId, List<Cart> cartItems, ISession session);
        Task <List<Cart>> LoadCartFromSession(string userId, ISession session);
    }
}
