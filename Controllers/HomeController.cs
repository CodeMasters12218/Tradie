using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

		public IActionResult Index()
		{
			return View();
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
