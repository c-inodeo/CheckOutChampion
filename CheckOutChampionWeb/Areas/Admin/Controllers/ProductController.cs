using CheckOutChampion.DataAccess.Data;
using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Models.ViewModels;
using CheckOutChampion.Services.Interface;
using CheckOutChampion.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckOutChampionWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index(string? searchString)
        {
            List<Product> categories = _productService.GetAllProducts();
            ViewData["CurrentFilter"] = searchString;
            return View(categories);
        }
        public IActionResult Upsert(int? id)
        {
            var product = id == null ? new Product() : _productService.GetProductById(id.Value);
            var productVM = new ProductVM
            {
                Product = product,
                CategoryList = _productService.GetCategoryList(),
                SelectedCategoryIds = product.Categories.Select(c => c.CategoryId).ToList()  // Load the selected category IDs
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _productService.UpsertProduct(productVM, file);
                TempData["success"] = "Product saved successfully!";
                return RedirectToAction("Index");
            }
            productVM.CategoryList = _productService.GetCategoryList();
            productVM.SelectedCategoryIds = productVM.Product.Categories.Select(c => c.CategoryId).ToList();

            return View(productVM);
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _productService.GetAllProducts();
            var result = products.Select(p => new
            {
                p.Id,
                p.ProductName,
                p.Description,
                p.Price,
                Categories = p.Categories.Select(pc => pc.Category.Name).ToList(),
                p.ImageUrl
            }).ToList();
            return Json(new { data = result });
        }
        public IActionResult Delete(int? id)
        {
            _productService.DeleteProduct(id.Value);
            return Json(new { success = true, message = "Delete Successful!" });
        }
        #endregion
    }
}
