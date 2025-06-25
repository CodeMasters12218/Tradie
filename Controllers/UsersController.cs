using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Tradie.Data;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userMgr;
		private readonly RoleManager<IdentityRole<int>> _roleMgr;
		//Logging
		private readonly ILogger<UsersController> _logger;
        private readonly IMemoryCache _cache;

		public UsersController(ApplicationDbContext context, UserManager<User> userMgr, RoleManager<IdentityRole<int>> roleMgr, ILogger<UsersController> logger, IMemoryCache cache)
		{
			_context = context;
			_userMgr = userMgr;
			_roleMgr = roleMgr;
			_logger = logger;
            _cache = cache;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string? searchTerm)
		{
            var usersQuery = _userMgr.Users
                .AsNoTracking()
                .Select(u => new AdminUserViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    LastNames = u.LastNames,
                    Email = u.Email,
                    ProfilePhotoUrl = u.ProfilePhotoUrl
                });


            if (!string.IsNullOrEmpty(searchTerm))
			{
				string lowerSearch = searchTerm!.ToLower();
				usersQuery = usersQuery.Where(u =>
					(u.Name != null && u.Name.ToLower().Contains(lowerSearch)) ||
					(u.Email != null && u.Email.ToLower().Contains(lowerSearch)));
			}

            int page = 1, pageSize = 20; 
            var pagedUsers = await usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var users = await usersQuery.ToListAsync();

			// Get logged admin
			var currentAdmin = await _userMgr.GetUserAsync(User);

			if (currentAdmin != null)
			{
				ViewData["AdminName"] = currentAdmin.Name;
				ViewData["AdminEmail"] = currentAdmin.Email;
				ViewData["AdminPhoto"] = currentAdmin.ProfilePhotoUrl;
			}
			else
			{
				// if not: fallback
				ViewData["AdminName"] = "Admin";
				ViewData["AdminEmail"] = "admin@example.com";
				ViewData["AdminPhoto"] = null;
			}

			var userViewModels = new List<AdminUserViewModel>();
            foreach (var user in pagedUsers)
            {
                var userEntity = await _userMgr.FindByIdAsync(user.Id.ToString());
                var roles = await _userMgr.GetRolesAsync(userEntity);
                var roleName = roles.FirstOrDefault();

                user.Role = roleName switch
                {
                    "Admin" => UserRole.Admin,
                    "Seller" => UserRole.Seller,
                    _ => UserRole.Customer
                };
                userViewModels.Add(user);
            }

            var model = new UserManagementViewModel
			{
				Users = userViewModels,
				CurrentUser = new AdminUserViewModel()
			};

			return View("~/Views/UserManagement/UserManagement.cshtml", model);
		}


		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null) return NotFound();

			return View(user);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserManagementViewModel vm)
        {
            var userVm = vm.CurrentUser;
            ModelState.Remove("Users");

            if (!ModelState.IsValid)
            {
                var modelStateErrors = string.Join(", ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine("Errores en ModelState: " + modelStateErrors);
                TempData["Error"] = "Hubo un problema al crear el usuario. Por favor, verifica los datos ingresados.";
                return RedirectToAction(nameof(Index));
            }

            var user = new User
            {
                Name = userVm.Name,
                LastNames = userVm.LastNames,
                UserName = userVm.Name,
                Email = userVm.Email,
                EmailConfirmed = true,
                Role = userVm.Role,  // si tienes esta propiedad en User
                CreatedAt = DateTime.UtcNow,
                // Otros campos que necesites
            };

            var result = await _userMgr.CreateAsync(user, userVm.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine("Errores al crear el usuario: " + errors);
                TempData["Error"] = errors;
                return RedirectToAction(nameof(Index));
            }

            var roleName = userVm.Role.ToString();
            if (!await _roleMgr.RoleExistsAsync(roleName))
            {
                await _roleMgr.CreateAsync(new IdentityRole<int>(roleName));
            }

            await _userMgr.AddToRoleAsync(user, roleName);

            TempData["Message"] = "Usuario creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserManagementViewModel vm)
        {
            var userVm = vm.CurrentUser;
            var user = await _userMgr.FindByIdAsync(userVm.Id.ToString());
            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Actualizar campos
            user.Name = userVm.Name;
            user.Email = userVm.Email;
            user.UserName = userVm.Email; // o userVm.Name según convenga

            var updateResult = await _userMgr.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["Error"] = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            // Actualizar roles
            var currentRoles = await _userMgr.GetRolesAsync(user);
            await _userMgr.RemoveFromRolesAsync(user, currentRoles);

            var newRole = userVm.Role.ToString();
            if (!await _roleMgr.RoleExistsAsync(newRole))
            {
                await _roleMgr.CreateAsync(new IdentityRole<int>(newRole));
            }

            await _userMgr.AddToRoleAsync(user, newRole);

            TempData["Message"] = "Usuario actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // Delete con SQL directo

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userMgr.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Obtener roles asignados al usuario
            var roles = await _userMgr.GetRolesAsync(user);

            // Quitar todos los roles asignados
            if (roles.Any())
            {
                var removeRolesResult = await _userMgr.RemoveFromRolesAsync(user, roles);
                if (!removeRolesResult.Succeeded)
                {
                    TempData["Error"] = "Error al quitar roles: " + string.Join(", ", removeRolesResult.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Index));
                }
            }

            // Ahora eliminar el usuario
            var deleteResult = await _userMgr.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                TempData["Error"] = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Usuario eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }


        private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
