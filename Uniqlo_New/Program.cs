using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Models;
using Uniqlo_New.Extensions;
using Uniqlo_New.Helpers;
using Uniqlo_New.Services.Abstracts;
using Uniqlo_New.Services.Implements;
using Microsoft.Extensions.DependencyInjection;

namespace Uniqlo_New
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<UniqloDBContextBp215>(opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSql")); });
			builder.Services.AddControllersWithViews();
			builder.Services.AddIdentity<User, IdentityRole>(opt =>
			{
				opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
				opt.User.RequireUniqueEmail=false;	
				opt.SignIn.RequireConfirmedEmail=true;
				opt.Password.RequiredLength = 5;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = true;
				opt.Lockout.MaxFailedAccessAttempts = 5;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDBContextBp215>();
		
			builder.Services.ConfigureApplicationCookie(x => { x.AccessDeniedPath = "/Home/AccesDenied"; });
			SmtpOptions options = new SmtpOptions();
			builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
			builder.Services.AddScoped<IEmailService, EmailService>(); 
			var app = builder.Build();
            app.UseStaticFiles();
			app.UseUserSeed();

			app.MapControllerRoute(name: "register", pattern: "register", defaults: new { controller = "Account", action = "Register" });

			app.MapControllerRoute(name: "areas", pattern: "{Area:exists}/{Controller=Home}/{Action=Index}/{id?}");
            app.MapControllerRoute(name:"default",pattern:"{Controller=Home}/{Action=Index}/{id?}");
			app.Run();
        }
    }
}
