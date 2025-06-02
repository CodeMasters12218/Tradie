using Tradie.Models.Products;

namespace Tradie.Models.Home
{
	public class HomeViewModel
	{
		// Section: HOME (e.g. 3 categories)
		public List<Product> RopaProducts { get; set; } = new();
		public List<Product> ElectronicaProducts { get; set; } = new();
		public List<Product> InformaticaProducts { get; set; } = new();


		// Section: OFERTAS (Deals / Discounts)
		public List<Product> OfertaProducts { get; set; } = new();


		// Section: Recién Llegados (New Arrivals)
		public List<Product> RecienLlegados { get; set; } = new();


		// Section: Famosos (Best Sellers / Popular)
		public List<Product> Famosos { get; set; } = new();
	}
}
