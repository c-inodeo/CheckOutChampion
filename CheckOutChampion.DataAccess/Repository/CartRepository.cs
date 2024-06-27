using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Cart> GetAllCartsByUserId(string userId)
        {
            return _context.CartItems.Where(c => c.UserId == userId).ToList();
        }

        public void AddToCart(Cart cart)
        {
            var existingCartItem = _context.CartItems
                .FirstOrDefault(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cart.Quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                _context.CartItems.Add(cart);
            }

            _context.SaveChanges();
        }

        public void Update(Cart cartItem)
        {
            var objFromDb = _context.CartItems.FirstOrDefault(u => u.CartId == cartItem.CartId);
            if (objFromDb != null)
            {
                objFromDb.Quantity = cartItem.Quantity;
                objFromDb.DateAdded = cartItem.DateAdded;
                _context.CartItems.Update(objFromDb);
            }
        }
    }
}
