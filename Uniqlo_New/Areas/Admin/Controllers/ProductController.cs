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
			if (vm.OtherFiles!=null && vm.OtherFiles.Any())
			{
				if (!vm.OtherFiles.All(x=>x.IsValidType("image")))
				{
					var filenames =vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName);
					ModelState.AddModelError("OtherFiles", string.Join(", ", filenames)+"are is not an image");
					string.Join(", ", filenames);	
				}

			}
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
		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
			if (!id.HasValue) return BadRequest();
			var data = await _context.Products
				.Where(p => p.Id == id.Value)
				.Select(x => new ProductUpdateVM
				{
					ProductName = x.Name,
					ProductDescription = x.Description,
					CostPrice = x.CostPrice,
					SellPrice = x.SellPrice,
					Discount = x.Discount,
					Quantity = x.Quantity,
					CategoryID = x.CategoryId,
					ImageUrl = x.CoverImage
				}).FirstOrDefaultAsync();
			if (data is null) return NotFound();
			return View(data);



		}
		[HttpPost]
		public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
		{
			if (!id.HasValue) return BadRequest();
			if (vm.CoverImage != null)
			{
				if (!vm.CoverImage.ContentType.StartsWith("image"))
					ModelState.AddModelError("File", "File type must be an image");
				if (vm.CoverImage.Length > 5 * 1024 * 1024)
					ModelState.AddModelError("File", "File must be less than 5mb");
			}
			if (!ModelState.IsValid)
			{
				ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
				return View(vm);
			}
			var products = await _context.Products
				.Where(p => p.Id == id.Value)
				.FirstOrDefaultAsync();
			if (products is null) return NotFound();
			if (vm.CoverImage != null)
			{
				string path = Path.Combine(_env.WebRootPath, "img", "products", products.CoverImage);
				using (Stream sr = System.IO.File.Create(path))
				{
					await vm.CoverImage!.CopyToAsync(sr);
				}
			}
			ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
			products.Name = vm.ProductName;
			products.Description = vm.ProductDescription;
			products.CostPrice = vm.CostPrice;
			products.SellPrice = vm.SellPrice;
			products.Quantity = vm.Quantity;
			products.Discount = vm.Discount;
			products.CategoryId = vm.CategoryID;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


	}
}
