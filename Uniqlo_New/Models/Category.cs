﻿namespace Uniqlo_New.Models
{
	public class Category:BaseEntity
	{
         public string Name { get; set; } = null!;
		 public IEnumerable<Product>? Products { get; set; }
    }
}
