using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task UpsertProduct(ProductVM productVM, IFormFile? file);
        Task DeleteProduct(int id);
        Task<IEnumerable<SelectListItem>> GetCategoryList();
        Task<string> TruncateText(string input, int length);
    }
}
