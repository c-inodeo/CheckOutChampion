using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Category>> GetCategories()
        {
            var getCategories = await _unitOfWork.Category.GetAll();
            return getCategories.ToList();
        }
        public async Task<Category> GetCategoryById(int id)
        {
            return await _unitOfWork.Category.Get(c => c.Id == id);
        }
        public async Task AddCategory(Category obj)
        {
            await _unitOfWork.Category.Add(obj);
            await _unitOfWork.Save();
        }
        public async Task UpdateCategory(Category obj)
        {
            await _unitOfWork.Category.Update(obj);
            await _unitOfWork.Save();
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _unitOfWork.Category.Get(c => c.Id == id);
            if (category != null)
            {
                await _unitOfWork.Category.Remove(category);
                await _unitOfWork.Save();
            }
        }
    }
}
