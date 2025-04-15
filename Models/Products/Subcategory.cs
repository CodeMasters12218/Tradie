namespace Tradie.Models
{
	public class Subcategory
	{
		public int Id { get; set; }
		public string? CategoryName { get; set; } // Link to the main category
		public string? SubCategoryName { get; set; }         // Camisetas, Zapatos...
		public string? ImageUrl { get; set; }

	}

}
