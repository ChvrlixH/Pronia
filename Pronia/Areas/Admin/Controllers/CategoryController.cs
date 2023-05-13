using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDb _appDb;

        public CategoryController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _appDb.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var category = await _appDb.Categories.Include(c=>c.Products).FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}
