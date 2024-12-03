using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Models;

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
				opt.Password.RequiredLength = 3;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = true;
				opt.Lockout.MaxFailedAccessAttempts = 1;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDBContextBp215>();


			var app = builder.Build();
            app.UseStaticFiles();
			app.MapControllerRoute(name: "register", pattern: "register", defaults: new { controller = "Account", action = "Register" });

			app.MapControllerRoute(name: "areas", pattern: "{Area:exists}/{Controller=Home}/{Action=Index}/{id?}");
            app.MapControllerRoute(name:"default",pattern:"{Controller=Home}/{Action=Index}/{id?}");
			app.Run();
        }
    }
}
