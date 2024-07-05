using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;

namespace CheckOutChampion.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IProductCategoryRepository ProductCategory { get; private set; }
        public UnitOfWork(ApplicationDbContext context) 
        { 
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
            Cart = new CartRepository(_context);
            ProductCategory = new ProductCategoryRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
