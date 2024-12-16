using Uniqlo_New.ViewModels.Product;



namespace Uniqlo_New.Models
{
	public class Product:BaseEntity
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; }=null!;
		public decimal CostPrice { get; set; }
		public decimal SellPrice { get; set; }
		public int Quantity { get; set; }
		public int  Discount { get; set; }
		public string CoverImage { get; set; } = null!;
		public int?  CategoryId { get; set; }
		public Category? Category { get; set; }
		public ICollection<ProductImage>? Images { get; set; }
		public ICollection<ProductRating>? Ratings { get; set; }
		public ICollection<ProductComment>? Comments { get; set; }
        public  ICollection<Tag>? Tags { get; set; }

        public static implicit operator Product(ProductCreateVM vm)
	 	{
			return new Product
			{ 
				Name = vm.Name,
				Description = vm.Description,
				CostPrice = vm.CostPrice,
				SellPrice = vm.SellPrice,
				Quantity = vm.Quantity,
				Discount = vm.Discount,
				CategoryId = vm.CategoryId,
			
			};


		}

	}
}
