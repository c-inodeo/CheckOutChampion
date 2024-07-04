﻿using CheckOutChampion.Models;
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
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        public void UpsertProduct(ProductVM productVM, IFormFile? file);
        public void DeleteProduct(int id);
        IEnumerable<SelectListItem> GetCategoryList();
        string TruncateText(string input, int length);
    }
}