using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.DataAccess.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }

        public IEnumerable<ProductCategory> GetAll(Expression<Func<ProductCategory, bool>>? filter = null)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories
            .Include(pc => pc.Product)
            .Include(pc => pc.Category);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }
    }
}
