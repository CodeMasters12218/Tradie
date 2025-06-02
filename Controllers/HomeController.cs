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

				RecienLlegados = await _context.Products
					.OrderByDescending(p => p.CreatedAt) // If you have CreatedAt field
					.Take(10).ToListAsync(),

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
	}

}
