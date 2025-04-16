using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tradie.Models;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
	public class ProductManagementController : Controller
	{
		private readonly ILogger<ProductManagementController> _logger;

		public ProductManagementController(ILogger<ProductManagementController> logger)
		{
			_logger = logger;
		}

		public IActionResult ProductRegistry()
		{
			ViewData["AdminName"] = "Admin X44";
			ViewData["AdminEmail"] = "admin@example.com";
			return View(new ProductManagement());
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
