using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services.Interface
{
    public interface ICategoryService
    {
        Task <List<Category>> GetCategories();
        Task <Category> GetCategoryById(int id);
        public Task AddCategory(Category obj);
        public Task UpdateCategory(Category obj);
        public Task DeleteCategory(int id);
    }
}