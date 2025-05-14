using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ILogger<CategoryController> _logger;
		private readonly ApplicationDbContext _context;

		public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Category()
		{
			// Sample subcategories grouped under categories
			var subcategories = new List<Subcategory>
			{
				new Subcategory { Id = 1001, CategoryName = "Ropa de Hombres", SubCategoryName = "Camisetas", ImageUrl = "/images/ropa_hombre/icons/tshirt.png" },
				new Subcategory { Id = 1002, CategoryName = "Ropa de Hombres", SubCategoryName = "Camisas", ImageUrl = "/images/ropa_hombre/icons/cloth.png" },
				new Subcategory { Id = 1003, CategoryName = "Ropa de Hombres", SubCategoryName = "Pantalones", ImageUrl = "/images/ropa_hombre/icons/jeans.png" },
				new Subcategory { Id = 1004, CategoryName = "Ropa de Hombres", SubCategoryName = "Zapatos", ImageUrl = "/images/ropa_hombre/icons/sneakers.png" },
				new Subcategory { Id = 1005, CategoryName = "Ropa de Hombres", SubCategoryName = "Mochilas", ImageUrl = "/images/ropa_hombre/icons/school-bag.png" },
				new Subcategory { Id = 1006, CategoryName = "Ropa de Hombres", SubCategoryName = "Acesorios", ImageUrl = "/images/ropa_hombre/icons/accessories_men.png" },
				new Subcategory { Id = 1007, CategoryName = "Ropa de Hombres", SubCategoryName = "Sudaderas", ImageUrl = "/images/ropa_hombre/icons/sweater.png" },

				new Subcategory { Id = 2001, CategoryName = "Ropa de Mujeres", SubCategoryName = "Camisetas", ImageUrl = "/images/ropa_mujer/icons/tshirt.png" },
				new Subcategory { Id = 2002, CategoryName = "Ropa de Mujeres", SubCategoryName = "Blusas", ImageUrl = "/images/ropa_mujer/icons/shirt.png" },
				new Subcategory { Id = 2003, CategoryName = "Ropa de Mujeres", SubCategoryName = "Pantalones", ImageUrl = "/images/ropa_mujer/icons/woman.png" },
				new Subcategory { Id = 2004, CategoryName = "Ropa de Mujeres", SubCategoryName = "Zapatos", ImageUrl = "/images/ropa_mujer/icons/high-heels.png" },
				new Subcategory { Id = 2005, CategoryName = "Ropa de Mujeres", SubCategoryName = "Mochilas", ImageUrl = "/images/ropa_mujer/icons/handbag.png" },
				new Subcategory { Id = 2006, CategoryName = "Ropa de Mujeres", SubCategoryName = "Acesorios", ImageUrl = "/images/ropa_mujer/icons/women_accessories.png" },
				new Subcategory { Id = 2007, CategoryName = "Ropa de Mujeres", SubCategoryName = "Sudaderas", ImageUrl = "/images/ropa_mujer/icons/sweater.png" },
				new Subcategory { Id = 2008, CategoryName = "Ropa de Mujeres", SubCategoryName = "Vestidos", ImageUrl = "/images/ropa_mujer/icons/dress.png" },
				new Subcategory { Id = 2009, CategoryName = "Ropa de Mujeres", SubCategoryName = "Fiesta", ImageUrl = "/images/ropa_mujer/icons/wedding-dress.png" },

				new Subcategory { Id = 3001, CategoryName = "Electrónica", SubCategoryName = "Camarás", ImageUrl = "/images/electronica/icons/camera.png" },
				new Subcategory { Id = 3002, CategoryName = "Electrónica", SubCategoryName = "Móviles", ImageUrl = "/images/electronica/icons/mobile-phone.png" },
				new Subcategory { Id = 3003, CategoryName = "Electrónica", SubCategoryName = "TV", ImageUrl = "/images/electronica/icons/television.png" },
				new Subcategory { Id = 3004, CategoryName = "Electrónica", SubCategoryName = "Audio", ImageUrl = "/images/electronica/icons/speaker.png" },
				new Subcategory { Id = 3005, CategoryName = "Electrónica", SubCategoryName = "Instrumentos musicales", ImageUrl = "/images/electronica/icons/live-music.png" },
				new Subcategory { Id = 3006, CategoryName = "Electrónica", SubCategoryName = "Wi-fi", ImageUrl = "/images/electronica/icons/wifi.png" },

				new Subcategory { Id = 4001, CategoryName = "Informática", SubCategoryName = "Portátiles", ImageUrl = "/images/informatica/icons/laptop.png" },
				new Subcategory { Id = 4002, CategoryName = "Informática", SubCategoryName = "Tablets", ImageUrl = "/images/informatica/icons/tablet.png" },
				new Subcategory { Id = 4003, CategoryName = "Informática", SubCategoryName = "Sobremesas", ImageUrl = "/images/informatica/icons/computer.png" },
				new Subcategory { Id = 4004, CategoryName = "Informática", SubCategoryName = "Gaming", ImageUrl = "/images/informatica/icons/gaming.png" },
				new Subcategory { Id = 4005, CategoryName = "Informática", SubCategoryName = "Monitores", ImageUrl = "/images/informatica/icons/monitor.png" },
				new Subcategory { Id = 4006, CategoryName = "Informática", SubCategoryName = "Componentes", ImageUrl = "/images/informatica/icons/motherboard.png" },
				new Subcategory { Id = 4007, CategoryName = "Informática", SubCategoryName = "Software", ImageUrl = "/images/informatica/icons/software.png" },

				new Subcategory { Id = 5001, CategoryName = "Kids & Toys", SubCategoryName = "Juguetes", ImageUrl = "/images/kids_toys/icons/bricks.png" },
				new Subcategory { Id = 5002, CategoryName = "Kids & Toys", SubCategoryName = "Juegos", ImageUrl = "/images/kids_toys/icons/board-game.png" },
				new Subcategory { Id = 5003, CategoryName = "Kids & Toys", SubCategoryName = "Bebé", ImageUrl = "/images/kids_toys/icons/baby-products.png" },
				new Subcategory { Id = 5004, CategoryName = "Kids & Toys", SubCategoryName = "Ropa de Bebé", ImageUrl = "/images/kids_toys/icons/baby_es.png" },

				new Subcategory { Id = 6001, CategoryName = "Oficina", SubCategoryName = "Materias de Oficina", ImageUrl = "/images/oficina/icons/desk-organizer.png" },
				new Subcategory { Id = 6002, CategoryName = "Oficina", SubCategoryName = "Papel", ImageUrl = "/images/oficina/icons/paper.png" },
				new Subcategory { Id = 6003, CategoryName = "Oficina", SubCategoryName = "Bolígrafos", ImageUrl = "/images/oficina/icons/pen.png" },
				new Subcategory { Id = 6004, CategoryName = "Oficina", SubCategoryName = "Impreso", ImageUrl = "/images/oficina/icons/printer.png" },
				new Subcategory { Id = 6005, CategoryName = "Oficina", SubCategoryName = "Tinta", ImageUrl = "/images/oficina/icons/printer-cartridges.png" },
				new Subcategory { Id = 6006, CategoryName = "Oficina", SubCategoryName = "Mochilas", ImageUrl = "/images/oficina/icons/briefcase.png" },

				new Subcategory { Id = 7001, CategoryName = "Hogar", SubCategoryName = "Cocina", ImageUrl = "/images/hogar/icons/kitchen.png" },
				new Subcategory { Id = 7002, CategoryName = "Hogar", SubCategoryName = "Comedor", ImageUrl = "/images/hogar/icons/chair.png" },
				new Subcategory { Id = 7003, CategoryName = "Hogar", SubCategoryName = "Dormitorio", ImageUrl = "/images/hogar/icons/bedroom.png" },
				new Subcategory { Id = 7004, CategoryName = "Hogar", SubCategoryName = "Salón", ImageUrl = "/images/hogar/icons/interior-design.png" },
				new Subcategory { Id = 7005, CategoryName = "Hogar", SubCategoryName = "Baño", ImageUrl = "/images/hogar/icons/public-toilet.png" },
				new Subcategory { Id = 7006, CategoryName = "Hogar", SubCategoryName = "Jardín", ImageUrl = "/images/hogar/icons/gardening.png" },
				new Subcategory { Id = 7007, CategoryName = "Hogar", SubCategoryName = "Iluminación", ImageUrl = "/images/hogar/icons/roof-lamp.png" },
				new Subcategory { Id = 7008, CategoryName = "Hogar", SubCategoryName = "pequeños electrodomésticos", ImageUrl = "/images/hogar/icons/microwave.png" },
				new Subcategory { Id = 7009, CategoryName = "Hogar", SubCategoryName = "grandes electrodomésticos", ImageUrl = "/images/hogar/icons/close.png" },

				new Subcategory { Id = 8001, CategoryName = "Libros", SubCategoryName = "Ficción", ImageUrl = "/images/libros/icons/science-fiction.png" },
				new Subcategory { Id = 8002, CategoryName = "Libros", SubCategoryName = "Consulta", ImageUrl = "/images/libros/icons/more-information.png" },
				new Subcategory { Id = 8003, CategoryName = "Libros", SubCategoryName = "Arte", ImageUrl = "/images/libros/icons/art-book.png" },
				new Subcategory { Id = 8004, CategoryName = "Libros", SubCategoryName = "Fotografía", ImageUrl = "/images/libros/icons/photography.png" },
				new Subcategory { Id = 8005, CategoryName = "Libros", SubCategoryName = "Historia", ImageUrl = "/images/libros/icons/history-book.png" },
				new Subcategory { Id = 8006, CategoryName = "Libros", SubCategoryName = "Humor", ImageUrl = "/images/libros/icons/joke.png" },
				new Subcategory { Id = 8007, CategoryName = "Libros", SubCategoryName = "Infantil", ImageUrl = "/images/libros/icons/education.png" },
				new Subcategory { Id = 8008, CategoryName = "Libros", SubCategoryName = "Ciencias", ImageUrl = "/images/libros/icons/scientific.png" },
				new Subcategory { Id = 8009, CategoryName = "Libros", SubCategoryName = "Medicina", ImageUrl = "/images/libros/icons/health.png" },
				new Subcategory { Id = 8010, CategoryName = "Libros", SubCategoryName = "Comics y Manga", ImageUrl = "/images/libros/icons/comic.png" },
				new Subcategory { Id = 8011, CategoryName = "Libros", SubCategoryName = "Deportes", ImageUrl = "/images/libros/icons/book.png" },
				new Subcategory { Id = 8012, CategoryName = "Libros", SubCategoryName = "Derecho", ImageUrl = "/images/libros/icons/law-book.png" },
				new Subcategory { Id = 8013, CategoryName = "Libros", SubCategoryName = "Economía", ImageUrl = "/images/libros/icons/financial.png" },

				new Subcategory { Id = 9001, CategoryName = "Videojuegos", SubCategoryName = "Xbox Series X y Y", ImageUrl = "/images/videojuegos/icons/xbox.png" },
				new Subcategory { Id = 9002, CategoryName = "Videojuegos", SubCategoryName = "Playstation 5 y 4", ImageUrl = "/images/videojuegos/icons/playstation.png" },
				new Subcategory { Id = 9004, CategoryName = "Videojuegos", SubCategoryName = "Nintendo Switch", ImageUrl = "/images/videojuegos/icons/nintendo-switch.png" },
				new Subcategory { Id = 9005, CategoryName = "Videojuegos", SubCategoryName = "Realidad Virtual", ImageUrl = "/images/videojuegos/icons/virtual-tour.png" },
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

		[Route("Category{name}/{subcategory}")]
		public IActionResult Subcategory(string name, string subcategory)
		{
			var products = Enumerable.Range(1, 20).Select(i => new Product
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

		// Details action to handle product details directly
		[Route("Category{name}/{subcategory}/{id}")]
		public IActionResult Details(string name, string subcategory, int id)
		{
			// First try to get product from database
			var product = _context.Products
				.Include(p => p.Reviews)
				.FirstOrDefault(p => p.Id == id);

			// If not found in database, generate a sample product
			if (product == null)
			{
				product = new Product
				{
					Id = id,
					Name = $"Producto de ejemplo {id}",
					Description = "Esta es una descripción detallada del producto. Incluye características, beneficios y otros detalles importantes.",
					Price = 20 + id,
					ImageUrl = id % 2 == 0 ? "/images/tshirt1.png" : "/images/tshirt2.png",
					Stock = 10 + id,
					Reviews = new List<UserProfileMainPage>() // Initialize empty reviews
				};
			}

			return View("~/Views/Product/Product.cshtml", product);
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
