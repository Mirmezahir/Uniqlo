using Microsoft.AspNetCore.Mvc;

namespace Uniqlo_New.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
