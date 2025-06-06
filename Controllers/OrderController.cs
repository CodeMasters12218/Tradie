using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Orders;

namespace Tradie.Controllers
{
	public class OrderController : BaseController
	{
		private readonly ApplicationDbContext _context;
		public OrderController(UserManager<User> userManager, ApplicationDbContext context)
			: base(userManager, context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Orders()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			var items = await _context.Orders
				.Include(o => o.Items)
					.ThenInclude(oi => oi.Product)
				.Where(o => o.CustomerId == user.Id)
				.SelectMany(o => o.Items)
				.ToListAsync();

			return View("~/Views/UserProfile/UserOrders.cshtml", items);
		}

		public async Task<IActionResult> UserOrdersProcessed()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var processedItems = await _context.Orders
				.Include(o => o.Items)
					.ThenInclude(oi => oi.Product)
				.Where(o => o.CustomerId == user.Id && o.Status == OrderStatus.Procesado)
				.SelectMany(o => o.Items)
				.ToListAsync();

			return View("~/Views/UserProfile/UserOrdersProcessed.cshtml", processedItems);
		}

		public async Task<IActionResult> UserOrdersDelivered()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var enviados = await _context.Orders
				.Include(o => o.Items)
					.ThenInclude(oi => oi.Product)
				.Where(o => o.CustomerId == user.Id && o.Status == OrderStatus.Enviado)
				.SelectMany(o => o.Items)
				.ToListAsync();

			return View("~/Views/UserProfile/UserOrdersDelivered.cshtml", enviados);
		}

	}
}
