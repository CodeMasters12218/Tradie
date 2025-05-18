using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tradie.Models.UserProfile;

namespace Tradie.Controllers
{
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// TEMP: Fake user — replace with real user logic later
			var user = new UserProfileMainPageModel
			{
				FullName = "Juan Pérez",
				Email = "juan.perez@example.com",
				ProfilePhotoUrl = "/images/girl_denim.png"
			};

			ViewBag.UserProfile = user;

			base.OnActionExecuting(context);
		}
	}
}
