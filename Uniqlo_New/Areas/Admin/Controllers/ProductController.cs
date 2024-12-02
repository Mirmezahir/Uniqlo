using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Extensions;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Product;

namespace Uniqlo_New.Areas.Admin.Controllers
{
		[Area("Admin")]
	public class ProductController(IWebHostEnvironment _env, UniqloDBContextBp215 _context) : Controller
	{
		public async Task<IActionResult> Index()
		{

			return View(await _context.Products.Where(x=> x.IsDeleted==false).Include(x=>x.Category).ToListAsync());
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.Categories= await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();	

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateVM vm)
		{
			if (vm.OtherFiles!=null &&) 
			if (vm.CoverFile != null)
			{
				if (vm.CoverFile.IsValidType("image")==false)
				{
					
					ModelState.AddModelError("CoverFile", "File type must be image");
				}
				if (!vm.CoverFile.IsValidSize(300))
				{
					ModelState.AddModelError("CoverFile", "File length must be less than 300kb");
				}
			}
			if (!ModelState.IsValid) { ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync(); return View(); }
			Product product = vm;
			product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath, "imgs", "products");
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete(int?id)
		{
			if (!id.HasValue) { return BadRequest(); }
			Product product = await _context.Products.FindAsync(id);
			product.IsDeleted= true;
			_context.Update(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
			


		}
		
	}
}
