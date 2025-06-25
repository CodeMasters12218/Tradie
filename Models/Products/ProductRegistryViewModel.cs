namespace Tradie.Models.Products
{
	public class ProductRegistryViewModel
	{
		public IEnumerable<ProductSummaryDto> Products { get; set; } = new List<ProductSummaryDto>();

		public Product NewProduct { get; set; } = new Product();

		public string? SearchTerm { get; set; }
		public IEnumerable<Category>? Categories { get; set; }

		public IEnumerable<User>? Sellers { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
