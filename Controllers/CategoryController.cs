using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Tradie.Models;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

		public IActionResult Index()
		{
			// Sample subcategories grouped under categories
			var subcategories = new List<Subcategory>
	{
		new Subcategory { Id = 1, CategoryName = "Ropa de Hombres", Name = "Camisetas", ImageUrl = "/images/tshirt1.png" },
		new Subcategory { Id = 2, CategoryName = "Ropa de Hombres", Name = "Zapatos", ImageUrl = "/images/shoes1.png" },
		new Subcategory { Id = 3, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/dress1.png" },
		new Subcategory { Id = 4, CategoryName = "Ropa de Mujeres", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
		new Subcategory { Id = 5, CategoryName = "Electrónica", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
		new Subcategory { Id = 6, CategoryName = "Electrónica", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
        // Add more here as needed
    };

			// Grouping subcategories under their main category
			var viewModel = subcategories
				.GroupBy(s => s.CategoryName)
				.Select(g => new CategoryWithSubcategoriesViewModel
				{
					CategoryName = g.Key,
					Subcategories = g.ToList()
				})
				.ToList();

			return View(viewModel);
		}

		[Route("category/{name}/{subcategory}")]
		public IActionResult Subcategory(string name, string subcategory)
		{
			var products = new List<Product>
	{
		new Product { Id = 1, Name = "Camiseta Blanca", Subcategory = subcategory, Description = "Camiseta de algodón blanca", Price = 19.99M, ImageUrl = "/images/tshirt1.png", Stock = 20 },
		new Product { Id = 2, Name = "Camiseta Negra", Subcategory = subcategory, Description = "Camiseta negra premium", Price = 24.99M, ImageUrl = "/images/tshirt2.png", Stock = 15 },
        // Add more products for testing...
    };

			ViewBag.Category = name;
			ViewBag.Subcategory = subcategory;

			return View(products);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
