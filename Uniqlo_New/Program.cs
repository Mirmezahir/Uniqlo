using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;

namespace Uniqlo_New
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<UniqloDBContextBp215>(opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSql")); });
			builder.Services.AddControllersWithViews();
            
            var app = builder.Build();
            app.UseStaticFiles();
            
			app.MapControllerRoute(name: "areas", pattern: "{Area:exists}/{Controller=Home}/{Action=Index}/{id?}");
            app.MapControllerRoute(name:"default",pattern:"{Controller=Home}/{Action=Index}/{id?}");
			app.Run();
        }
    }
}
