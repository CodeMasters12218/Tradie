using Microsoft.AspNetCore.Mvc;
using Tradie.Models.UserProfile; // Needed to access UserProfileMainPageModel

namespace Tradie.Controllers
{
	public class UserProfileController : Controller
	{
		public IActionResult UserProfileMainPage()
		{
			var model = new UserProfileMainPageModel
			{
				FullName = "Diego Sánchez",
				Email = "example@gmail.com",
				Orders = new List<string> { "Pedido 1", "Pedido 2" }
			};

			return View(model); // Assumes there's a corresponding view at Views/UserProfile/UserProfileMainPage.cshtml
		}

		// Simulating data fetching; in real case, you'd fetch by logged-in user ID
		public IActionResult UserEditProfile()
		{
			// Example data, should be retrieved from DB/service
			var model = new UserEditProfileModel
			{
				FullName = "Juan Pérez",
				Email = "juan@example.com",
				PhoneNumber = "123456789",
				Address = "Calle Falsa 123"
			};

			return View(model); // Looks for Views/UserProfile/EditProfile.cshtml
		}

		[HttpPost]
		public IActionResult SaveProfile(UserEditProfileModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("EditProfile", model);
			}

			// Save logic here (e.g., database update)

			// Redirect back to main profile page after save
			return RedirectToAction("Index", "UserProfile");
		}

	}
}
