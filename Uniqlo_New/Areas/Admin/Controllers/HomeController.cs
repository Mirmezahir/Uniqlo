using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniqlo_New.Enums;

namespace Uniqlo_New.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
