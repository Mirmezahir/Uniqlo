using Microsoft.EntityFrameworkCore;
using Uniqlo_New.Models;

namespace Uniqlo_New.DataAccess
{
	public class UniqloDBContextBp215:DbContext
	{
        public DbSet<Sliders> Slider { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }

        public UniqloDBContextBp215(DbContextOptions opt) : base(opt) { }
	}
}
