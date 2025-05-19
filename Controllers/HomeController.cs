using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tradie.Models;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
		: base(userManager)
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
