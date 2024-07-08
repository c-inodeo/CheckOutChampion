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

        public IEnumerable<Product> GetProducts(string? searchString)
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Categories.Category");
            if (!string.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     p.Categories.Any(c => c.Category.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
            }

            return productList.Select(p => new Product
            {
                Id = p.Id,
                ProductName = _productService.TruncateText(p.ProductName, 15),
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Categories = p.Categories.Select(pc => new ProductCategory
                {
                    Category = new Category { Name = _productService.TruncateText(pc.Category.Name, 15) }
                }).ToList()

            });
        }
        public Product GetProductDetails(int id)
        {
            return _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Categories.Category");
        }
        public string TruncateText(string input, int length)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= length) 
            {
                return input;
            }
            return input.Substring(0, length) + "...";
        }
    }
}
