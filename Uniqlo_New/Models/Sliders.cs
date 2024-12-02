using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.Models
{
	public class Sliders : BaseEntity
	{
		[MaxLength(32)]
		public string Title { get; set; } = null!;
		[MaxLength(64)]
		public string Subtitle { get; set; } = null!;
		[MaxLength(64)]
		public string? Link { get; set; }
		public string ImageUrl { get; set; } = null!;
	}
}
