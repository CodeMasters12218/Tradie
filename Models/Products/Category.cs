namespace Tradie.Models.Products
{
	public class Category
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? ImageUrl { get; set; }
		public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
	}
}
