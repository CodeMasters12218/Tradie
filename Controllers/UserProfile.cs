using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.UserAddress;
using Tradie.Models.UserCards;
using Tradie.Models.UserProfile;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class UserProfileController : BaseController
	{
        private readonly ApplicationDbContext _context;
        public UserProfileController(UserManager<User> userManager, ApplicationDbContext context)
            : base(userManager, context) => _context = context;

        public async Task<IActionResult> UserProfileMainPage()
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var user = await GetCurrentUserAsync();

				if (user != null)
				{
					var model = new UserProfileMainPageModel
					{
						FullName = user.Name ?? "No Name",
						Email = user.Email,
						ProfilePhotoUrl = !string.IsNullOrEmpty(user.ProfilePhotoUrl)
					? user.ProfilePhotoUrl
					: "/images/boy_black.png",
						Orders = new List<string> { "Pedido 1", "Pedido 2" } // Replace with actual logic
					};

					return View(model);
				}
			}

			// If user is not authenticated, redirect to login
			return RedirectToAction("Login", "Account");
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

		public IActionResult UserDeleteProfile()
		{
			return View("UserDeleteProfile"); // Looks for Views/UserProfile/UserDeleteProfile.cshtml
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


        public async Task<IActionResult> UserCards()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View("~/Views/UserProfile/UserCards.cshtml");
            }

            return RedirectToAction("Index", "UserCardProfile", new AdminUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                LastNames = user.LastNames,
                Role = user.Role,
                Password = user.PasswordHash,
                UserCardProfile = new UserCardProfileModel(),
				UserAddressProfile = new UserAddressProfileModel()
            });

        }

		public async Task<IActionResult> UserAddressAsync()
		{
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View("~/Views/UserProfile/UserCards.cshtml");
            }

            return RedirectToAction("Index", "UserAddress", new AdminUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                LastNames = user.LastNames,
                Role = user.Role,
                Password = user.PasswordHash,
                UserCardProfile = new UserCardProfileModel(),
                UserAddressProfile = new UserAddressProfileModel()
            });
		}
    }
}
