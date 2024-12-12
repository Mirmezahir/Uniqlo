using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Migrations;
using Uniqlo_New.Models;

namespace Uniqlo_New.Controllers
{
	public class ProductController(UniqloDBContextBp215 _contex) : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (!id.HasValue)
			{
				return BadRequest();
			}
			var data = await _contex.Products.Where(x=> x.Id == id .Value && !x.IsDeleted).Include(x=> x.Images).Include(x=>x.Ratings).ThenInclude(x=>x.User).FirstOrDefaultAsync();
			if (data == null) return NotFound();
			ViewBag.Rating = 5;
			if (User.Identity?.IsAuthenticated ?? false)
			{
				string userId = User.Claims.FirstOrDefault(x=> x.Type== ClaimTypes.NameIdentifier)!.Value;
			  int rating =	await _contex.ProductRatings.Where(x=> x.UserId== userId && x.ProductId == id).Select(x=> x.Rating).FirstOrDefaultAsync();
				ViewBag.Rating = rating == 0 ? 5 : rating; 
			}
			ViewBag.Comments = await _contex.ProductComments.ToListAsync();
			return View(data);
		}
		public async Task<IActionResult> Rating(int productId,int rating)
		{
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
			var data = await _contex.ProductRatings.Where(x=>x.UserId== userId && x.ProductId==productId).FirstOrDefaultAsync();
			if (data == null)
			{
				await _contex.ProductRatings.AddAsync(new Models.ProductRating { 
				UserId = userId,
				Rating = rating,
				ProductId = productId
				
				});

			}
			else
			{
				data.Rating = rating;
			}
			await _contex.SaveChangesAsync();
			return RedirectToAction(nameof(Details), new {Id = productId });
		}
		public async Task<IActionResult> Comment(int productId,string comment)
		{
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
			var data = await _contex.ProductComments.Where(x=> x.UserId==userId&& x.ProductId==productId).FirstOrDefaultAsync();
			await _contex.ProductComments.AddAsync(new Models.ProductComment { 
			UserId=userId,
			Comment = comment,
			ProductId=productId
			});
			await _contex.SaveChangesAsync();


			return RedirectToAction(nameof(Details), new { Id = productId });
		}
	}
}
