
namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDb _context;

        public HomeController(AppDb context)
        {
            _context = context;
        }
    
        public IActionResult Index()
        {
            List<Info> FAQ = _context.FAQ.ToList();
            List<Slider> sliders = _context.Sliders.ToList();


            HomeVM homeVM = new()
            {
                infos = FAQ,
                Sliders= sliders
            };

            return View(homeVM);

        }
    }
}
