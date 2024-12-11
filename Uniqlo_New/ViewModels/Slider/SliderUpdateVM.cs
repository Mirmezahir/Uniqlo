namespace Uniqlo_New.ViewModels.Slider
{
    public class SliderUpdateVM
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Link   { get; set; }
        public string ImageUrl { get; set; }
		public IFormFile? CoverImage { get; set; }
	}
}
