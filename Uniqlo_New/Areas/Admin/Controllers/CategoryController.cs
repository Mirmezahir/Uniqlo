using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Categories;

namespace Uniqlo_New.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class Category(UniqloDBContextBp215 _context) : Controller
	{
		public async Task<IActionResult> Index()
		{
			var data = await _context.Categories.Where(x=> !x.IsDeleted).ToListAsync();
			return View(data);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateVM vm)
		{
			if (vm.Name == null) { ModelState.AddModelError("Name", "Name must be fill"); }
			if (ModelState.IsValid)
			{
				Models.Category categories = new() { Name = vm.Name };
				await _context.Categories.AddAsync(categories);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (!id.HasValue) { return BadRequest(); }
			var data = await _context.Categories.FindAsync(id);
			if (data is null) { return BadRequest(); }
			data.IsDeleted = true;
			await _context.SaveChangesAsync();


			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			 var data = await _context.Categories.FindAsync(id);
			if (data is null) { return BadRequest(); }
			CategoryUpdateVM vm = new CategoryUpdateVM {Name=data.Name };




			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Update(CategoryUpdateVM vm,int id)
		{
			var data = await _context.Categories.FindAsync(id);
			data.Name = vm.Name;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}




	}
}
