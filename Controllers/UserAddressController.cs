using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
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
        public IActionResult Index(AdminUserViewModel user)
        {
            int userID = user.Id;
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
    }
}
