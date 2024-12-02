using System.ComponentModel.DataAnnotations;

namespace Uniqlo_New.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        [MaxLength(32)]
        public string Name { get; set; } = null!;
    }
}
