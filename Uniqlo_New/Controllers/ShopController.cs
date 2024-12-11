using Microsoft.AspNetCore.Mvc;

namespace Uniqlo_New.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
