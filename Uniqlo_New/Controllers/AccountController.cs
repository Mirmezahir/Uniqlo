using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using Uniqlo_New.Enums;
using Uniqlo_New.Helpers;
using Uniqlo_New.Models;
using Uniqlo_New.Services.Abstracts;
using Uniqlo_New.ViewModels.Auths;

namespace Uniqlo_New.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> _singInManager,IOptions<SmtpOptions> options,IEmailService _service) : Controller
    {
        readonly SmtpOptions smtpOptions=options.Value;
        public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (IsAuthenticated) { return RedirectToAction("Index", "Home"); }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            if (IsAuthenticated) { return RedirectToAction("Index","Home"); }
            if(!ModelState.IsValid)
            {
                return View();
            }
            User user = new User() { UserName=vm.Username, Email=vm.Email,Fullname=vm.FullName,ProfileImageUrl="photo.jpg"};
           var result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            var roleResult = await _userManager.AddToRoleAsync(user,nameof(Roles.User));
             if (!roleResult.Succeeded)
            {
                foreach(var item in roleResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
             string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _service.SendEmailConfirmation(user.Email, user.UserName, token);
            return Content("Email Send");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) { return View(); }
            User? user = null;
            if (vm.UserNameOrEmail.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            }
            else
                user= await _userManager.FindByNameAsync(vm.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("","Username or password is wrong");
                return View();
            }
           var result =  await _singInManager.PasswordSignInAsync(user,vm.Password,vm.RememberMe,true);
            if (!result.Succeeded)
            {
                if(result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Wait until"+user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return View();
            } 

                return RedirectToAction("Index", "Home");
          
       
        }
        [Authorize]
         public async Task<IActionResult> Logout()
         {
           await _singInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
         }
        //public async Task<IActionResult> Test()
        //{
        //    SmtpClient smtp = new();
        //    smtp.Host = smtpOptions.Host;
        //    smtp.Port = smtpOptions.Port;
        //    smtp.EnableSsl = true;
        //    smtp.Credentials = new NetworkCredential(smtpOptions.Username,smtpOptions.Password);
        //    MailAddress from = new MailAddress(smtpOptions.Username,
                
                
        //        "Uniqlo");
        //    MailAddress to = new("ismayilzademirmezahir@gmail.com");
        //    MailMessage message = new MailMessage(from,to);
        //    message.Subject= "Test";
        //     message.Body = "<p>Test body <a>Link</a> </p>";
        //    message.IsBodyHtml = true;
        //    smtp.Send(message);
        //    return Ok();
        //}
        public async Task<IActionResult> VerifyEmail(string token,string user)
        {
           var entity= await _userManager.FindByNameAsync(user);
            if (entity == null) { return BadRequest(); }
            var result =await _userManager.ConfirmEmailAsync(entity,token.Replace(" ","+"));
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    sb.AppendLine(item.Description);
                }
                return Content(sb.ToString());
            }
            await _singInManager.SignInAsync(entity,true);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid) { return View(); }
            User? user = null;
            var data = await _userManager.FindByEmailAsync(vm.Email);
            user = data;
            if (user == null) {
                ModelState.AddModelError("","Istifadeci tapilmadi");
                return View(); }
            user.EmailConfirmed = false;
            user.UserName = data!.UserName;
            user.Email = data!.Email;   
           await _userManager.RemovePasswordAsync(user);
           await _userManager.AddPasswordAsync(user, vm.Password);
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _service.SendEmailConfirmation(user.Email, user.UserName, token);
            
          
            return RedirectToAction(nameof(CheckGmail));
        }
        public IActionResult CheckGmail()
        {
            return View();
        }
      
    
    }
}
