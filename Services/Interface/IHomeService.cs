using CheckOutChampion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services.Interface
{
    public interface IHomeService
    {
        Task <IEnumerable<Product>> GetProducts(string? searchString);
        Task <Product> GetProductDetails(int id);
        string TruncateText(string input, int length);
    }
}
