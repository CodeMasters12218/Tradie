using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tradie.Data;
using Tradie.Models;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext context)
		: base(userManager, context)
		{
			_logger = logger;
		}

		private readonly ILogger<HomeController> _logger;

		public async Task<IActionResult> Index()
		{
			// Get the logged-in user
			var user = await _userManager.GetUserAsync(User);
			var wishlistProductIds = new List<int>();

			// If user is logged in, fetch their wishlist product IDs
			if (user != null)
			{
				var wishlist = await _context.Wishlists
					.Include(w => w.Items)
					.FirstOrDefaultAsync(w => w.UserId == user.Id.ToString());

				if (wishlist != null)
				{
					wishlistProductIds = wishlist.Items.Select(i => i.ProductId).ToList();
				}
			}

			// for categories
			var schema = _context.Model.FindEntityType(typeof(Product));
			var columns = schema.GetProperties().Select(p => p.Name);
			Console.WriteLine(string.Join(", ", columns));

			// Set ViewBag so the _ProductCard.cshtml can use it
			ViewBag.WishlistProductIds = wishlistProductIds;

			// Build the HomeViewModel
			var vm = new Tradie.Models.Home.HomeViewModel

			{
				RopaProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("ropa"))
					.Take(10).ToListAsync(),

				ElectronicaProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("electrónica"))
					.Take(10).ToListAsync(),

				InformaticaProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("informática"))
					.Take(10).ToListAsync(),

				/* Oferta Part */
				OficinaProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("oficina") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				HogarProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("hogar") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				LibrosProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("libros") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				VideojuegosProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("videojuegos") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				KidsProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("kids") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				RopamujerProducts = await _context.Products
					.Where(p => p.Category != null && p.Category.Name.ToLower().Contains("ropa mujer") && p.DiscountPercentage > 0)
					.Take(2)
					.ToListAsync(),

				RecienLlegados = await _context.Products
					.OrderByDescending(p => p.DateAdded)
					.Take(20).ToListAsync(),

				/*
				Famosos = await _context.Products
					.OrderByDescending(p => p.SalesCount)
					.Take(10).ToListAsync(),
				*/
			};

			return View(vm);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		// For recien llegados to return 20 products
		public async Task<IActionResult> RecienLlegados(int count = 20)
		{
			var recentProducts = await _context.Products
				.Include(p => p.Seller)
				.OrderByDescending(p => p.DateAdded)
				.Take(count)
				.ToListAsync();

			return View(recentProducts);
		}
	}

}
