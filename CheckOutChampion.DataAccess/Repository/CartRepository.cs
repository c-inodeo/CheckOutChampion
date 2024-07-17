using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CheckOutChampion.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAll(Expression<Func<Cart, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Cart> query = _context.CartItems;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query =  query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public async Task AddToCart(Cart cart)
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
                await _context.CartItems.AddAsync(cart);
            }

            _context.SaveChanges();
        }

        public async Task Update(Cart cartItem)
        {
            var objFromDb = await _context.CartItems.FirstOrDefaultAsync(u => u.CartId == cartItem.CartId);
            if (objFromDb != null)
            {
                objFromDb.Quantity = cartItem.Quantity;
                objFromDb.DateAdded = cartItem.DateAdded;
                _context.CartItems.Update(objFromDb);
            }
        }
    }
}
