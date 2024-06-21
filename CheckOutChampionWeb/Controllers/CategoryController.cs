using CheckOutChampionWeb.Data;
using CheckOutChampionWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckOutChampionWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
           _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            }
            if (ModelState.IsValid) 
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _context.Categories.Find(id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id ==id);
            //Category? categoryFromDb2 = _context.Categories.Where(u=> u.Id == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _context.Categories.Find(id);
            //Category? categoryFromDb1 = _context.Categories.FirstOrDefault(u => u.Id ==id);
            //Category? categoryFromDb2 = _context.Categories.Where(u=> u.Id == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _context.Categories.Find(id);
            if (obj == null) 
            {
                return NotFound();
            }
            _context.Categories.Remove(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
