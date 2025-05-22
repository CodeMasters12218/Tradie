using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;

public class CartBadgeViewComponent : ViewComponent
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<User> _userManager;

	public CartBadgeViewComponent(ApplicationDbContext context, UserManager<User> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		int itemCount = 0;

		if (User.Identity != null && User.Identity.IsAuthenticated)
		{
			var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
			if (user != null)
			{
				var cart = await _context.ShoppingCarts
					.Include(c => c.Items)
					.FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

				itemCount = cart?.Items.Sum(i => i.Quantity) ?? 0;
			}
		}

		return View(itemCount);
	}
}
