using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckOutChampion.Models;

namespace CheckOutChampion.Services.Interface
{
    public interface ICartService
    {
        List<Cart> GetCartItems(string userId);
        void AddOrUpdateCartItem(string userId, int productId, int quantity, bool isIncrement);
        void RemoveCartItem(int cartId);
        double GetTotalPrice(string userId);
    }
}
