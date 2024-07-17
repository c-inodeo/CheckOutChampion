using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using CheckOutChampion.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
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
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly ILogger<ProductService> _logger;


        public ProductService(IUnitOfWork unitOfWork, 
            IWebHostEnvironment webHostEnvironment,
            IAzureBlobStorageService azureBlobStorageService,
            ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _azureBlobStorageService = azureBlobStorageService;
            _logger = logger;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Product.Get(c => c.Id == id);    
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var allProducts = await _unitOfWork.Product.GetAll(includeProperties: "Categories.Category");
            return allProducts.ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoryList()
        {
            var categoryList = await _unitOfWork.Category.GetAll();
            var selectListItems = categoryList.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return selectListItems;
        }

        public async Task UpsertProduct(ProductVM productVM, IFormFile? file)
        {
            var product = productVM.Product;

            // Handle image upload
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    await _azureBlobStorageService.DeleteFileAsync(product.ImageUrl);
                }
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        try
                        {
                            product.ImageUrl = await _azureBlobStorageService.UploadFileAsync(memoryStream, filename);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while uploading the file.");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while handling the file upload.");
                    throw;
                }
            }


            foreach (var categoryId in productVM.SelectedCategoryIds)
            {
                var categoryExists = await _unitOfWork.Category.Get(c => c.Id == categoryId);
                if (categoryExists == null)
                {
                    throw new InvalidOperationException($"Category ID {categoryId} does not exist in the database.");
                }
            }

            // Create Product
            if (product.Id == 0)
            {
                if (string.IsNullOrEmpty(product.ProductName))
                {
                    throw new InvalidOperationException("Product Name is required.");
                }

                await _unitOfWork.Product.Add(product);
                _unitOfWork.Save();

                foreach (var categoryId in productVM.SelectedCategoryIds)
                {
                    await _unitOfWork.ProductCategory.Add(new ProductCategory { ProductId = product.Id, CategoryId = categoryId });
                }

            }
            // Update Product
            else
            {
                await _unitOfWork.Product.Update(product);
                _unitOfWork.Save();

                var existingCategories = await _unitOfWork.ProductCategory.GetAll(pc => pc.ProductId == product.Id);
                foreach (var category in existingCategories.ToList())
                {
                    await _unitOfWork.ProductCategory.Remove(category);
                }

                foreach (var categoryId in productVM.SelectedCategoryIds)
                {
                    var categoryExists = await _unitOfWork.Category.Get(c => c.Id == categoryId);
                    if (categoryExists == null)
                    {
                        throw new InvalidOperationException($"Category ID {categoryId} does not exist in the database.");
                    }

                    await _unitOfWork.ProductCategory.Add(new ProductCategory { ProductId = product.Id, CategoryId = categoryId });
                }
            }

            _unitOfWork.Save();
        }

        public async Task DeleteProduct(int id)
        {
            var productToBeDeleted = await _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(productToBeDeleted.ImageUrl))
                {
                    await _azureBlobStorageService.DeleteFileAsync(productToBeDeleted.ImageUrl);
                }

                await _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save();
            }
        }

        public async Task<string> TruncateText(string input, int length)
        {
            if (string.IsNullOrEmpty(input)|| input.Length <= length)
            { 
                return input;
            }
            await Task.Yield();
            return input.Substring(0, length) + "...";
        }

    }
}
