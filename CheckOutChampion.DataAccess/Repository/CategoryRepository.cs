using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;

namespace CheckOutChampion.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Category obj)
        {
            _context.Categories.Update(obj);
        }
    }
}
