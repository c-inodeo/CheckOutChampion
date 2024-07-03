using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using CheckOutChampion.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
          _unitOfWork = unitOfWork;
          _webHostEnvironment = webHostEnvironment;
        }

        public Product GetProductById(int id)
        {
            return _unitOfWork.Product.Get(c => c.Id == id);    
        }

        public List<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll(includeProperties: "CategoryNav").ToList();
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            return _unitOfWork.Category.GetAll().Select( c => new SelectListItem
            { 
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void UpsertProduct(ProductVM productVM, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null) 
            { 
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productImagePath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) 
                {
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) 
                    { 
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productImagePath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                productVM.Product.ImageUrl = @"\images\product\" + filename;
            }
            if (productVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
        }
        public void DeleteProduct(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save();
            }
        }

    }
}
