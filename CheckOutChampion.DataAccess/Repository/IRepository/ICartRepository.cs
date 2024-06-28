using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CheckOutChampion.DataAccess.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        IEnumerable<Cart> GetAll(Expression<Func<Cart, bool>>? filter = null, string? includeProperties = null);
        void AddToCart(Cart cart);
        void Update(Cart cartItem);
    }
}
