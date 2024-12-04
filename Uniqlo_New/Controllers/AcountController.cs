using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Auths;


namespace Pronia.Controllers
{
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager) : Controller
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

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]


        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View();
            User? user = null;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is wrong!");
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                if (!result.IsLockedOut)
                {
                    ModelState.AddModelError("", "wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
