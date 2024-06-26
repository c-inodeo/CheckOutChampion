using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.DataAccess.Repository.IRepository
{
    public interface ICartRepository :IRepository<Cart>
    {
        IEnumerable<Cart> GetAllCartsByUserId(string userId);
        void AddToCart(Cart cart);
    }
}
