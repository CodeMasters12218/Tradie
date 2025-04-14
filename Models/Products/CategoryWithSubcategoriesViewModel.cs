namespace Tradie.Models
{
	public class CategoryWithSubcategoriesViewModel
	{
		public string CategoryName { get; set; } = "";
		public List<Subcategory> Subcategories { get; set; } = new();
	}
}
