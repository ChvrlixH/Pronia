using Pronia.Areas.Admin.ViewModels;
using Pronia.Utils;
using Pronia.Utils.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pronia.Areas.Admin.Controllers
{
   [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _appDb.Sliders.AsEnumerable();

            ViewBag.Count = sliders.Count();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (sliderVM.Image == null)
            {
                ModelState.AddModelError("Image", "Image bos olmamalidir.");
                return View();
            }
          
            if (!sliderVM.Image.CheckFileSize(100))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                return View();
            }
            if (!sliderVM.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View();
            }

            string fileName = $"{Guid.NewGuid()}-{sliderVM.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "admin","images", "faces", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await sliderVM.Image.CopyToAsync(stream);
            }

            Slider slider = new()
            {
                Name = sliderVM.Name,
                Description = sliderVM.Description,
                Offer = sliderVM.Offer,
                Image = fileName
            };

            await _appDb.Sliders.AddAsync(slider);
            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Read(int id)
        {
            Slider? slider = _appDb.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _appDb.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleteslider(int id)
        {
            Slider? slider = await _appDb.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider is null)
                return NotFound();

            //string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "admin", "images", "faces", slider.Image);
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}

            FileSlider.DeleteFile(_webHostEnvironment.WebRootPath, "assets", "admin", "images", "faces", slider.Image);

            _appDb.Sliders.Remove(slider);
            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider? slider = _appDb.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Slider slider, int id)
        {
            Slider? dBslider = _appDb.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            IEnumerable<Slider> sliders = _appDb.Sliders.AsEnumerable();
            foreach (var sliderItem in sliders)
            {
                if (sliderItem.Name == slider.Name)
                {
                    ModelState.AddModelError("Name", "This name is available");
                    return View();
                }
            }

            _appDb.Sliders.Update(slider);
            _appDb.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

