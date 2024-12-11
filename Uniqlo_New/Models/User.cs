using Microsoft.AspNetCore.Identity;

namespace Uniqlo_New.Models
{
	public class User : IdentityUser
	{
		public string Fullname { get; set; }
		public string ProfileImageUrl { get; set; }

		
	}
}
