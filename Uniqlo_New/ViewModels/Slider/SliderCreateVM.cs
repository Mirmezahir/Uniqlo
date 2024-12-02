using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.ViewModels.Slider
{
	public class SliderCreateVM
	{
        [MaxLength(32,ErrorMessage ="Title length must be less than 32"),Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
		[MaxLength(64, ErrorMessage = "Subtitle length must be less than 32"), Required(ErrorMessage = "Subtitle is required")]
		public  string Subtitle { get; set; }

        public string? Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
