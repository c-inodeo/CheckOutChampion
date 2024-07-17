using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CheckOutChampion.DataAccess.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task <IEnumerable<ProductCategory>> GetAll(Expression<Func<ProductCategory, bool>>? filter = null);
    }
}
