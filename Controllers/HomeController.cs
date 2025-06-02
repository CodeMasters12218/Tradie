using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tradie.Data;
using Tradie.Models;

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
			// Step 1: Get the logged-in user
			var user = await _userManager.GetUserAsync(User);
			var wishlistProductIds = new List<int>();

			// Step 2: If user is logged in, fetch their wishlist product IDs
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

			// Step 3: Set ViewBag so the _ProductCard.cshtml can use it
			ViewBag.WishlistProductIds = wishlistProductIds;

			// Step 4: Build the HomeViewModel
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

				/*
				OfertaProducts = await _context.Products
					.Where(p => p.Discount > 0) // If you have a Discount field
					.OrderByDescending(p => p.Discount)
					.Take(10).ToListAsync(), 
				*/

				RecienLlegados = await _context.Products
					.OrderByDescending(p => p.DateAdded)
					.Take(10).ToListAsync(),

				/*
				Famosos = await _context.Products
					.OrderByDescending(p => p.SalesCount) // If you track sales
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


		// For recien llegados to return 10 products
		public async Task<IActionResult> RecienLlegados(int count = 10)
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
