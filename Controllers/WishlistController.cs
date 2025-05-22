using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.ShoppingCart;

namespace Tradie.Controllers
{
	public class WishlistController : BaseController
	{
		private readonly ApplicationDbContext _context;

		public WishlistController(
			ApplicationDbContext context,
			UserManager<User> userManager
		) : base(userManager)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			// You should ideally store a Wishlist per user (one-to-many)
			var wishlist = _context.Wishlists
				.Include(w => w.Items)
				.ThenInclude(i => i.Product)
				.FirstOrDefault(w => w.UserId == user.Id.ToString()); // Assuming you add a UserId to Wishlist

			if (wishlist == null)
			{
				wishlist = new Models.Wishlist.Wishlist
				{
					UserId = user.Id.ToString()
				};
				_context.Wishlists.Add(wishlist);
				await _context.SaveChangesAsync();
			}

			return View(wishlist);
		}

		[HttpPost]
		public async Task<IActionResult> RemoveItem(int itemId)
		{
			var item = await _context.WishlistItems.FindAsync(itemId);

			if (item != null)
			{
				_context.WishlistItems.Remove(item);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(int itemId)
		{
			var item = await _context.WishlistItems
				.Include(i => i.Product)
				.FirstOrDefaultAsync(i => i.Id == itemId);

			if (item == null)
				return NotFound();

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login", "Account");

			// Example: Add to cart logic (replace with your implementation)
			var cart = _context.ShoppingCarts
				.Include(c => c.Items)
				.FirstOrDefault(c => c.UserId == user.Id.ToString());

			if (cart == null)
			{
				cart = new ShoppingCart { UserId = user.Id.ToString() };
				_context.ShoppingCarts.Add(cart);
				await _context.SaveChangesAsync();
			}

			cart.Items.Add(new CartItem
			{
				ProductId = item.ProductId,
				Quantity = item.Quantity,
				PriceAtAddition = item.PriceAtAddition,
				Size = item.Size,
				Color = item.Color
			});

			_context.WishlistItems.Remove(item);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

	}
}
