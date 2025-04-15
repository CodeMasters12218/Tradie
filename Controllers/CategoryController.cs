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
				new Subcategory { Id = 1001, CategoryName = "Ropa de Hombres", Name = "Camisetas", ImageUrl = "/images/girl_denim.png" },
				new Subcategory { Id = 1002, CategoryName = "Ropa de Hombres", Name = "Camisas", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 1003, CategoryName = "Ropa de Hombres", Name = "Pantalones", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 1004, CategoryName = "Ropa de Hombres", Name = "Zapatos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 1005, CategoryName = "Ropa de Hombres", Name = "Mochilas", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 1006, CategoryName = "Ropa de Hombres", Name = "Acesorios", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 1007, CategoryName = "Ropa de Hombres", Name = "Acesorios", ImageUrl = "/images/boy_white.png" },

				new Subcategory { Id = 2001, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 2002, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 2003, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 2004, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 2005, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },
				new Subcategory { Id = 2006, CategoryName = "Ropa de Mujeres", Name = "Vestidos", ImageUrl = "/images/boy_white.png" },

				new Subcategory { Id = 3001, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
				new Subcategory { Id = 3002, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
				new Subcategory { Id = 3003, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
				new Subcategory { Id = 3004, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
				new Subcategory { Id = 3005, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },
				new Subcategory { Id = 3006, CategoryName = "Electrónica", Name = "Blusas", ImageUrl = "/images/blouse1.png" },

				new Subcategory { Id = 4001, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
				new Subcategory { Id = 4002, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
				new Subcategory { Id = 4003, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
				new Subcategory { Id = 4004, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
				new Subcategory { Id = 4005, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },
				new Subcategory { Id = 4006, CategoryName = "Infromática", Name = "Smartphones", ImageUrl = "/images/phone1.png" },

				new Subcategory { Id = 5001, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5002, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5003, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5004, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5005, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5006, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 5007, CategoryName = "Kids & Toys", Name = "Auriculares", ImageUrl = "/images/headphones.png" },

				new Subcategory { Id = 6001, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6002, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6003, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6004, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6005, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6006, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 6007, CategoryName = "Oficina", Name = "Auriculares", ImageUrl = "/images/headphones.png" },

				new Subcategory { Id = 7001, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 7002, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 7003, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 7004, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 7005, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 7006, CategoryName = "Hogar", Name = "Auriculares", ImageUrl = "/images/headphones.png" },

				new Subcategory { Id = 8001, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8002, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8003, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8004, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8005, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8006, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8007, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 8008, CategoryName = "Libros", Name = "Auriculares", ImageUrl = "/images/headphones.png" },

				new Subcategory { Id = 9001, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9002, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9003, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9004, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9005, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9006, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9007, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9008, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
				new Subcategory { Id = 9009, CategoryName = "Videojuegos", Name = "Auriculares", ImageUrl = "/images/headphones.png" },
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
			var products = Enumerable.Range(1, 12).Select(i => new Product
			{
				Id = i,
				Name = $"{subcategory} Producto {i}",
				Subcategory = subcategory,
				Description = $"Este es el producto número {i} en {subcategory}.",
				Price = 10 + i,
				ImageUrl = i % 2 == 0 ? "/images/tshirt1.png" : "/images/tshirt2.png",
				Stock = 10 + i
			}).ToList();

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
