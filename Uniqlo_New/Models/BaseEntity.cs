namespace Uniqlo_New.Models
{
	public class BaseEntity
	{
		public int Id { get; set; }
		public DateTime CreateTime { get; set; } = DateTime.Now;
		public bool IsDeleted { get; set; }
	}
}
