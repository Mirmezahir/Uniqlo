using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.ViewModels.Categories
{
	public class CategoryUpdateVM
	{
		[MaxLength(32), Required(ErrorMessage = "Must be fill")]
		public string Name { get; set; } = null!;
    }
}
