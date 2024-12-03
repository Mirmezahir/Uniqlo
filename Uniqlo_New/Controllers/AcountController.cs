using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Auths;

namespace Uniqlo_New.Controllers
{
	public class AccountController(UserManager<User> userManager) : Controller
	{
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]

		public async Task<IActionResult> Register(UserCreateVM vm)
		{
			if (!ModelState.IsValid)
				return View();
			User user = new User
			{
				Email = vm.Email,
				Fullname = vm.Fullname,
				UserName = vm.Username,
				ProfileImageUrl = "photo.jpg"
			};
			var result = await userManager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View();
			}
			return View();
		}
	}
}
