using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tradie.Models.UserProfile;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class BaseController : Controller
	{
		protected readonly UserManager<User> _userManager;

		public BaseController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var userId = _userManager.GetUserId(User);
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
			}

			base.OnActionExecuting(context);
		}

		protected async Task<User?> GetCurrentUserAsync()
		{
			return await _userManager.GetUserAsync(User);
		}
	}
}
