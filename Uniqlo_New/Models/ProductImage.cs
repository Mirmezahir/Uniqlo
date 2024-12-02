using System.Reflection.Metadata.Ecma335;

namespace Uniqlo_New.Models
{
	public class ProductImage:BaseEntity
	{
        public string FileUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
