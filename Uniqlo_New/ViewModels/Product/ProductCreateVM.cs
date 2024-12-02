using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.ViewModels.Product
{
	public class ProductCreateVM
	{
		[MaxLength(32),Required(ErrorMessage ="Must be fill")]
		public string Name { get; set; } = null!;

		[MaxLength(64),Required(ErrorMessage ="Must be fill")]
		public string Description { get; set; } = null!;
		[Required(ErrorMessage = "Must be fill")]
		public decimal CostPrice { get; set; }
		[Required(ErrorMessage = "Must be fill")]
		public decimal SellPrice { get; set; }
		[Required(ErrorMessage = "Must be fill")]
		public int Quantity { get; set; }
		[Required(ErrorMessage = "Must be fill")]
		public int Discount { get; set; }
		[Required(ErrorMessage = "Must be fill")]
		public IFormFile CoverFile { get; set; } 
		public IEnumerable<IFormFile> OtherFiles { get; set; }
		[Required(ErrorMessage = "Must be fill")]
		public int? CategoryId { get; set; }
		
	}
}
