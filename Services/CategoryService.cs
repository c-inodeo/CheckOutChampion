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
        public List<Category> GetCategories()
        {
            return _unitOfWork.Category.GetAll().ToList();
        }
        public Category GetCategoryById(int id)
        {
            return _unitOfWork.Category.Get(c => c.Id == id);
        }
        public void AddCategory(Category obj)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
        }
        public void UpdateCategory(Category obj)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
        }
        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
            }
        }
    }
}
