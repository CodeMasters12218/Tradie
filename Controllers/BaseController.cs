using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tradie.Data;
using Tradie.Models.UserProfile;

namespace Tradie.Controllers
{
	public class BaseController : Controller
	{
		protected readonly UserManager<User> _userManager;
		private readonly ApplicationDbContext _context;

		public BaseController(UserManager<User> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var userId = _userManager.GetUserId(User);

				// Load User Profile for layout/header
				if (int.TryParse(userId, out int id))
				{
					var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
					if (user != null)
					{
						ViewBag.UserProfile = new UserProfileMainPageModel
						{
							FullName = user.Name ?? "No Name",
							Email = user.Email,
							ProfilePhotoUrl = !string.IsNullOrEmpty(user.ProfilePhotoUrl)
								? user.ProfilePhotoUrl
								: "/images/boy_black.png"
						};
					}
				}

				// Load Wishlist Product IDs
				var wishlistProductIds = _context.WishlistItems
					.Where(w => w.Wishlist.UserId == userId)
					.Select(w => w.ProductId)
					.ToList();

				ViewBag.WishlistProductIds = wishlistProductIds;

				// For Add to Cart functionality
				var cart = _context.ShoppingCarts
					.Where(c => c.UserId == userId)
					.SelectMany(c => c.Items)
					.Select(i => i.ProductId)
					.ToList();

				ViewBag.CartProductIds = cart;


			}

			base.OnActionExecuting(context);
		}

		protected async Task<User?> GetCurrentUserAsync()
		{
			return await _userManager.GetUserAsync(User);
		}
	}
}
