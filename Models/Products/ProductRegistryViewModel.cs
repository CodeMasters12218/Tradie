namespace Tradie.Models.Products
{
	public class ProductRegistryViewModel
	{
		public IEnumerable<Product> Products { get; set; } = new List<Product>();

		public Product NewProduct { get; set; } = new Product();

		public string? SearchTerm { get; set; }
		public IEnumerable<Category>? Categories { get; set; }

		public IEnumerable<User>? Sellers { get; set; }
	}
}
