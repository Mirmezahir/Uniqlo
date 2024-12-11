using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Uniqlo_New.DataAccess;
using Uniqlo_New.ViewModels.Common;
using Uniqlo_New.ViewModels.Product;
using Uniqlo_New.ViewModels.Slider;

namespace Uniqlo_New.Controllers
{
    public class HomeController(UniqloDBContextBp215 _cnontext) : Controller
    {
        public async Task<IActionResult> Index()
        {
             HomeVM vm = new HomeVM();
            vm.Sliders= await _cnontext.Slider.Where(x => x.IsDeleted == false).Select(x => new SliderItemVM
			{
				ImageUrl = x.ImageUrl,
				Link = x.Link,
				Subtitle = x.Subtitle,
				Title = x.Title,
			}).ToListAsync();
            vm.Products= await _cnontext.Products.Where(x=>x.IsDeleted==false).Select(x=> new ProductItemVM
            { 
                Discount = x.Discount,
                Name = x.Name,
                Id = x.Id,
                ImageUrl=x.CoverImage,
                IsInStock=x.Quantity>0,
                Price=x.SellPrice,
            }
            ).ToListAsync();
			
            return View(vm);
        }
        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
