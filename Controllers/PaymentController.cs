using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Payments;

namespace Tradie.Controllers
{
	public class PaymentController : BaseController
	{
		private readonly ApplicationDbContext _context;

		public PaymentController(
			UserManager<User> userManager,
			ApplicationDbContext context
		) : base(userManager)
		{
			_context = context;
		}

		public async Task<IActionResult> PaymentDetails()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null || !cart.Items.Any())
				return RedirectToAction("Index", "ShoppingCart");

			var model = new PaymentDetails
			{
				Name = user.Name,
				EmailAddress = user.Email,
				Items = cart.Items
			};

			return View(model);
		}

		public async Task<IActionResult> PaymentMethod()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null || !cart.Items.Any())
				return RedirectToAction("Index", "ShoppingCart");

			var model = new PaymentMethod
			{
				Items = cart.Items
			};

			return View(model);
		}


		public async Task<IActionResult> PaymentSummary()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null || !cart.Items.Any())
				return RedirectToAction("Index", "ShoppingCart");

			var model = new PaymentSummary
			{
				Items = cart.Items
			};

			return View(model);
		}

	}
}
