using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Threading;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InfoController : Controller
    {
        private readonly AppDb _context;

        public InfoController(AppDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Info> faq = _context.FAQ.ToList();
            ViewBag.Count = faq.Count;

            return View(faq);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(Info info)
        {
            if (!ModelState.IsValid)
                return View();


            if (_context.FAQ.Count() > 2)
            {
                Thread.Sleep(2000);
                return BadRequest("Info is Full");
            }



                List<Info> faq = _context.FAQ.ToList();
            foreach (var InfoItem in faq)
            {
                if (InfoItem.Title == info.Title)
                {
                    ModelState.AddModelError("Title", "This title is available");
                    return View();
                }
            }


            _context.FAQ.Add(info);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Read(int id)
        {
            Info? info = _context.FAQ.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }

        public IActionResult Delete(int id)
        {
            Info? info = _context.FAQ.FirstOrDefault(s => s.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteInfo(int id)
        {
            Info? info = _context.FAQ.FirstOrDefault(s => s.Id == id);
            if (info is null)
                return NotFound();

            _context.FAQ.Remove(info);
            _context.SaveChanges();

           return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Info? info = _context.FAQ.FirstOrDefault(s => s.Id == id);
            if (info is null)
                return NotFound();

            return View(info);
        }


        [HttpPost]
        public IActionResult Update(Info info, int id)
        {
            Info? dBinfo = _context.FAQ.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (info is null)
                return NotFound();

            List<Info> faq = _context.FAQ.ToList();
            foreach (var InfoItem in faq)
            {
                if (InfoItem.Title == info.Title)
                {
                    ModelState.AddModelError("Title", "This title is available");
                    return View();
                }
            }

            _context.FAQ.Update(info);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
