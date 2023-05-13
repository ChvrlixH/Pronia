using Pronia.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDb _appDb;

        public ShopController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            IQueryable<Product> products = _appDb.Products.AsQueryable();

            ViewBag.ProductsCount = await products.CountAsync();

            ShopVM shopVM = new()
            {
                Products = categoryId !=null ? await products.Where(p=>p.CategoryId==categoryId).ToListAsync() 
                : await products.ToListAsync(),
                Categories = await _appDb.Categories.Include(c=>c.Products).Where(p => !p.IsDeleted).ToListAsync(),
            };

            return View(shopVM);
        }
    }
}
