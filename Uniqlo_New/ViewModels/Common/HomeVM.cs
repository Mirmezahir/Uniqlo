using Uniqlo_New.ViewModels.Product;
using Uniqlo_New.ViewModels.Slider;

namespace Uniqlo_New.ViewModels.Common
{
	public class HomeVM
	{
		public IEnumerable<SliderItemVM> Sliders { get; set; }
		public IEnumerable<ProductItemVM> Products { get; set; }	
    }
}
