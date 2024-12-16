using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Basket;

namespace Uniqlo_New.Controllers
{
	public class BasketController(UniqloDBContextBp215 _context, UserManager<User> _userManager, SignInManager<User> _singInManager) : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Add(int id)
		{
			if (!await _context.Products.AnyAsync(x => x.Id == id))
			{
				return NotFound();
			}
			var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");
			var item = basketItems.FirstOrDefault(x => x.Id == id);
			if (item == null)
			{
				item = new BasketProductItemVM { Id = id, Count = 1 };
		     	basketItems.Add(item);
			}
			else
				item.Count++;
			
			Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));


			return Ok();
		}
	}
}
