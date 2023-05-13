using Pronia.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDb _appDb;

        public ProductController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _appDb.Products.Include(p => p.Category).OrderByDescending(p=>p.ModifiedAt).ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _appDb.Categories.AsEnumerable();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProductVM productVM)
        {
            ViewBag.Categories = _appDb.Categories.AsEnumerable();

            if (!ModelState.IsValid)
                return View();

            if (!_appDb.Categories.Any(c => c.Id == productVM.CategoryId))
                return BadRequest(RedirectToAction(nameof(Create)));

            Product product = new()
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Rating = productVM.Rating,
                Image = productVM.Image,
                CategoryId = productVM.CategoryId,
                IsDeleted = false
            };

            await _appDb.Products.AddAsync(product);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Read(int id)
        {
            Product? product = await _appDb.Products.AsNoTracking().Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) { return NotFound(); }

            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            Product? product = await _appDb.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) { return NotFound(); }

            ViewBag.Categories = _appDb.Categories.Where(c => !c.IsDeleted);

            ProductVM productVM = new()
            {
                Name = product.Name,
                Price = product.Price,
                Rating = product.Rating,
                Image = product.Image,
                CategoryId = product.CategoryId,
            };

            return View(productVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, ProductVM productVM)
        {
            ViewBag.Categories = _appDb.Categories.Where(c => !c.IsDeleted);
            if (!ModelState.IsValid)
            {
                return View();
            }
        
            if (!_appDb.Categories.Any(c => c.Id == productVM.CategoryId))
                return BadRequest(RedirectToAction(nameof(Create)));

            Product? product = await _appDb.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product is null) { return NotFound(); }

            product.Name= productVM.Name;
            product.Price= productVM.Price;
            product.Rating= productVM.Rating;
            product.Image= productVM.Image;
            product.CategoryId = productVM.CategoryId;

            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var products = await _appDb.Products.Include(p=>p.Category).FirstOrDefaultAsync(p=>p.Id==id);
            if (products is null) { return NotFound();}

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _appDb.Products.FirstOrDefaultAsync(p=>p.Id==id);
            if(product is null) { return NotFound();}

            product.IsDeleted = true;

            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
