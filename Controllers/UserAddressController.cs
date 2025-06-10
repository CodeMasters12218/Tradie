using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Tradie.Data;
using Tradie.Models.Paypal;
using Tradie.Models.UserAddress;
using Tradie.Models.UserAddressModel;
using Tradie.Models.UserCards;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class UserAddressController : BaseController
	{
		private readonly ApplicationDbContext _context;
		public UserAddressController(UserManager<User> userManager, ApplicationDbContext context)
			: base(userManager, context)
		{
			_context = context;
		}
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var addresses = _context.UserAddress.Where(address => address.UserId == user.Id).ToList();

            AdminUserViewModel model = new AdminUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                LastNames = user.LastNames,
                Role = user.Role,
                UserCardProfile = new UserCardProfileModel(),
                UserAddressProfile = new UserAddressProfileModel
                {
                    Addresses = addresses,
                    CurrentAddress = new UsersAddressModel()
                }
            };

            return View("~/Views/UserProfile/UserAddress.cshtml", model);
        }

        [HttpPost]
		public async Task<IActionResult> CreateAddress(AdminUserViewModel model)
		{
			var user = await GetCurrentUserAsync();
			UsersAddressModel address = model.UserAddressProfile.CurrentAddress;
			if (model == null)
			{
				ModelState.AddModelError("", "No se recibieron datos para la dirección.");
				return View("~/Views/UserProfile/UserAddress.cshtml");
			}

			address.UserId = user.Id;
			_context.UserAddress.Add(address);
			_context.SaveChanges();

			AdminUserViewModel AdminUser = new AdminUserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				LastNames = user.LastNames,
				Role = user.Role,
				Password = user.PasswordHash,
				UserCardProfile = new UserCardProfileModel(),
				UserAddressProfile = new UserAddressProfileModel
				{
					Addresses = _context.UserAddress.Where(address => address.UserId == user.Id).ToList(),
					CurrentAddress = new UsersAddressModel()
				},
			};

			return View("~/Views/UserProfile/UserAddress.cshtml", AdminUser);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(string addressId)
        {
            try
            {
                if (string.IsNullOrEmpty(addressId))
                {
                    TempData["Error"] = "No se encontró la dirección para eliminar.";
                    return RedirectToAction(nameof(Index));
                }

                var addressToDelete = await _context.UserAddress.FirstOrDefaultAsync(a => a.Id == addressId);

                if (addressToDelete == null)
                {
                    TempData["Error"] = "No se encontró la dirección para eliminar.";
                    return RedirectToAction(nameof(Index));
                }

                _context.UserAddress.Remove(addressToDelete);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Dirección eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar dirección: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
