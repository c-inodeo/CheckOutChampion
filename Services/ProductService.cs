using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using CheckOutChampion.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
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
            return _unitOfWork.Product.GetAll(includeProperties: "Categories.Category").ToList();
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
            var product = productVM.Product;

            // Handle image upload
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productImagePath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productImagePath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                product.ImageUrl = @"\images\product\" + filename;
            }
            foreach (var categoryId in productVM.SelectedCategoryIds)
            {
                var categoryExists = _unitOfWork.Category.Get(c => c.Id == categoryId);
                if (categoryExists == null)
                {
                    throw new InvalidOperationException($"Category ID {categoryId} does not exist in the database.");
                }
            }
            if (product.Id == 0)
            {
                if (string.IsNullOrEmpty(product.ProductName))
                {
                    throw new InvalidOperationException("Product Name is required.");
                }

                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();  

                foreach (var categoryId in productVM.SelectedCategoryIds)
                {
                    _unitOfWork.ProductCategory.Add(new ProductCategory { ProductId = product.Id, CategoryId = categoryId });
                }

            }
            else
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();

            }
            var existingCategories = _unitOfWork.ProductCategory.GetAll(pc => pc.ProductId == product.Id).ToList();
            foreach (var category in existingCategories)
            {
                _unitOfWork.ProductCategory.Remove(category);
            }

            foreach (var categoryId in productVM.SelectedCategoryIds)
            {
                var categoryExists = _unitOfWork.Category.Get(c => c.Id == categoryId);
                if (categoryExists == null)
                {
                    throw new InvalidOperationException($"Category ID {categoryId} does not exist in the database.");
                }

                _unitOfWork.ProductCategory.Add(new ProductCategory { ProductId = product.Id, CategoryId = categoryId });
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

        public string TruncateText(string input, int length)
        {
            if (string.IsNullOrEmpty(input)|| input.Length <= length)
            { 
                return input;
            }
            return input.Substring(0, length) + "...";
        }
    }
}
