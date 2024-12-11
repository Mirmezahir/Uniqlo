using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.DataAccess;
using Uniqlo_New.Enums;
using Uniqlo_New.Models;
using Uniqlo_New.ViewModels.Slider;

namespace Uniqlo_New.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class SliderController(UniqloDBContextBp215 _context,IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
           var data =await _context.Slider.ToListAsync();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Create(SliderCreateVM vm)
		{
           
            if (!vm.File.ContentType.StartsWith("image"))
            ModelState.AddModelError("File", "File type must be image");
            if (vm.File.Length > 600 * 1024)
            ModelState.AddModelError("File", "File Length must be less than 600kb");
			if (!ModelState.IsValid) return View();
            string newFileNAme = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath,"imgs","sliders",newFileNAme)))
            {
               await vm.File.CopyToAsync(stream);
            }
            Sliders slider = new Sliders
            {
                ImageUrl=newFileNAme,
                Link=vm.Link,
                Subtitle=vm.Subtitle,
                Title=vm.Title,
            };
            await _context.Slider.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


          
		}

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) { return BadRequest(); }
            var data = await _context.Slider.FindAsync(id);
            if (data==null){ return BadRequest(); };
            string OldFile = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","imgs","sliders",data.ImageUrl);
            if (System.IO.File.Exists(OldFile))
            {
                System.IO.File.Delete(OldFile);
            }
            _context.Slider.Remove(data);
            await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
        [HttpGet]
        public async Task<IActionResult> Update(int?id)
        {
            var data = await _context.Slider.FindAsync(id);
            if (data == null) { return BadRequest(); }
            SliderUpdateVM vm = new SliderUpdateVM();
            vm.Subtitle = data.Subtitle;
            vm.Title = data.Title;  
            vm.ImageUrl = data.ImageUrl;
            vm.Link = data.Link;
            return View(vm);
            
        }
        [HttpPost]
		public async Task<IActionResult> Update(SliderUpdateVM vm,int? id)
		{
           var data =await _context.Slider.FindAsync(id);
            data.Subtitle = vm.Subtitle;
            data.Title = vm.Title;
            data.ImageUrl = vm.ImageUrl;
            data.Link = vm.Link;
           await _context.SaveChangesAsync();
            
		
			return RedirectToAction(nameof(Index));

		}

	}
}
