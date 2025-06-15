using Tradie.Models.Products;

namespace Tradie.Models.Home
{
	public class HomeViewModel
	{
		// Section: HOME (3 categories)
		public List<Product> RopaProducts { get; set; } = new();
		public List<Product> ElectronicaProducts { get; set; } = new();
		public List<Product> InformaticaProducts { get; set; } = new();


		// Section: OFERTAS
		public List<Product> OficinaProducts { get; set; } = new();
		public List<Product> HogarProducts { get; set; } = new();
		public List<Product> LibrosProducts { get; set; } = new();
		public List<Product> VideojuegosProducts { get; set; } = new();
		public List<Product> KidsProducts { get; set; } = new();
		public List<Product> RopamujerProducts { get; set; } = new();

		// Section: Recién Llegados
		public List<Product> RecienLlegados { get; set; } = new List<Product>();


		// Section: Famosos
		public List<Product> Famosos { get; set; } = new();
	}
}
