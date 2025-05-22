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

		[HttpPost]
		public async Task<IActionResult> AddToCart(int productId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				return NotFound();

			var cart = _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefault(c => c.UserId == user.Id.ToString());

			if (cart == null)
			{
				cart = new ShoppingCart { UserId = user.Id.ToString() };
				_context.ShoppingCarts.Add(cart);
			}

			cart.AddItem(product, 1); // Use your existing AddItem logic

			await _context.SaveChangesAsync();

			return RedirectToAction("Subcategory", "Category", new
			{
				name = product.Subcategory,
				subcategory = product.Subcategory
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveItem(int itemId)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null) return RedirectToAction("Index");

			var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
			if (item != null)
			{
				cart.Items.Remove(item);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		private bool ShoppingCartExists(int id)
		{
			return _context.ShoppingCarts.Any(e => e.Id == id);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateQuantity([FromBody] QuantityUpdateDto dto)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return Unauthorized();

			var cart = await _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

			if (cart == null) return NotFound();

			var item = cart.Items.FirstOrDefault(i => i.Id == dto.ItemId);
			if (item == null) return NotFound();

			item.Quantity = dto.Quantity;
			await _context.SaveChangesAsync();

			return Json(new { success = true, subtotal = cart.Subtotal, total = cart.Total });
		}

		public class QuantityUpdateDto
		{
			public int ItemId { get; set; }
			public int Quantity { get; set; }
		}

	}
}
