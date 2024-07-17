using CheckOutChampion.DataAccess.Repository;
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
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public HomeService(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public async Task<IEnumerable<Product>> GetProducts(string? searchString)
        {
            var productList = await _unitOfWork.Product.GetAll(includeProperties: "Categories.Category");

            if (!string.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     p.Categories.Any(c => c.Category.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
            }

            var productListTasks = productList.Select(async p => new Product
            {
                Id = p.Id,
                ProductName = await _productService.TruncateText(p.ProductName, 15),
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Categories = await Task.WhenAll(p.Categories.Select(async pc => new ProductCategory
                {
                    Category = new Category { Name = await _productService.TruncateText(pc.Category.Name, 15) }
                }))
            });

            return await Task.WhenAll(productListTasks);
        }

        public async Task<Product> GetProductDetails(int id)
        {
            var getProductDetails = await _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Categories.Category");
            return getProductDetails;
        }
    }
}
