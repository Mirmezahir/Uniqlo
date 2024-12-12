using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniqlo_New.Models;

namespace Uniqlo_New.DataAccess
{
	public class UniqloDBContextBp215:IdentityDbContext<User>
	{
        public DbSet<Sliders> Slider { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<ProductRating> ProductRatings { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductComment> ProductComments { get; set; }

        public UniqloDBContextBp215(DbContextOptions<UniqloDBContextBp215> opt) : base(opt) { }
	}
}
