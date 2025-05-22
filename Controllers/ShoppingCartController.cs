using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.ShoppingCart;

namespace Tradie.Controllers
{
	public class ShoppingCartController : BaseController
	{
		private readonly ApplicationDbContext _context;

		public ShoppingCartController(
			ApplicationDbContext context,
			UserManager<User> userManager)
			: base(userManager)
		{
			_context = context;
		}

		// GET: ShoppingCarts
		// GET: ShoppingCarts
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.ThenInclude(i => i.Product)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null)
			{
				cart = new ShoppingCart { UserId = user.Id.ToString() };
				_context.ShoppingCarts.Add(cart);
				await _context.SaveChangesAsync();
			}

			return View("~/Views/ShoppingCart/ShoppingCart.cshtml", cart);
		}

		// GET: ShoppingCarts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var shoppingCart = await _context.ShoppingCarts
				.FirstOrDefaultAsync(m => m.Id == id);
			if (shoppingCart == null)
			{
				return NotFound();
			}

			return View(shoppingCart);
		}

		// GET: ShoppingCarts/Create
		public IActionResult Create()
		{
			return View();
		}


		// POST: ShoppingCarts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id")] ShoppingCart shoppingCart)
		{
			if (ModelState.IsValid)
			{
				_context.Add(shoppingCart);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(shoppingCart);
		}

		// GET: ShoppingCarts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
			if (shoppingCart == null)
			{
				return NotFound();
			}
			return View(shoppingCart);
		}

		// POST: ShoppingCarts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id")] ShoppingCart shoppingCart)
		{
			if (id != shoppingCart.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(shoppingCart);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ShoppingCartExists(shoppingCart.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(shoppingCart);
		}

		// GET: ShoppingCarts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var shoppingCart = await _context.ShoppingCarts
				.FirstOrDefaultAsync(m => m.Id == id);
			if (shoppingCart == null)
			{
				return NotFound();
			}

			return View(shoppingCart);
		}

		// POST: ShoppingCarts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
			if (shoppingCart != null)
			{
				_context.ShoppingCarts.Remove(shoppingCart);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ShoppingCartExists(int id)
		{
			return _context.ShoppingCarts.Any(e => e.Id == id);
		}
	}
}
