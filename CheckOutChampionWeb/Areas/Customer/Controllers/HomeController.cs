using CheckOutChampion.DataAccess.Repository.IRepository;
using CheckOutChampion.Models;
using CheckOutChampion.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CheckOutChampionWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IProductService productService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public IActionResult Index(string? searchString)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "CategoryNav,Categories.Category");

            if (!String.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     p.CategoryNav.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                     p.Categories.Any(c => c.Category.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)));                                                     
            }

            ViewData["CurrentFilter"] = searchString;
           
            var truncateProductName = productList.Select(c => new Product
            {
                Id = c.Id,
                ProductName = _productService.TruncateText(c.ProductName, 15),
                CategoryNav = new Category { Name = _productService.TruncateText(c.CategoryNav.Name, 15) },
                Price = c.Price,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Categories = c.Categories
            });

            return View(truncateProductName);
        }
        public IActionResult Details(int? id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "CategoryNav");
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
