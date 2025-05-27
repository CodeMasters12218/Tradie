using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		public UsersController(ApplicationDbContext context, UserManager<User> userMgr, RoleManager<IdentityRole<int>> roleMgr, ILogger<UsersController> logger)
		{
			_context = context;
			_userMgr = userMgr;
			_roleMgr = roleMgr;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string? searchTerm)
		{
			var usersQuery = _userMgr.Users.AsQueryable();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				string lowerSearch = searchTerm!.ToLower();
				usersQuery = usersQuery.Where(u =>
					u.Name != null && u.Name.ToLower().Contains(lowerSearch) ||
					u.Email != null && u.Email.ToLower().Contains(lowerSearch));
			}

			var users = await usersQuery.ToListAsync();

			var userViewModels = new List<AdminUserViewModel>();
			foreach (var user in users)
			{
				var roleName = (await _userMgr.GetRolesAsync(user)).FirstOrDefault();
				userViewModels.Add(new AdminUserViewModel
				{
					Name = user.Name,
					LastNames = user.LastNames,
					Role = roleName switch
					{
						"Admin" => UserRole.Admin,
						"Seller" => UserRole.Seller,
						_ => UserRole.Customer
					},
					Id = user.Id,
					Email = user.Email,
					ProfilePhotoUrl = user.ProfilePhotoUrl
				});
				Console.WriteLine(user.LastNames);

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


		/*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserManagementViewModel vm)
        {
            var userVm = vm.CurrentUser;

            ModelState.Remove("Users");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = userVm.Name,
                    UserName = userVm.Name,
                    Email = userVm.Email,
                    Role = userVm.Role,
                    EmailConfirmed = true

                };
                try
                {

                    var result = await _userMgr.CreateAsync(user, userVm.Password);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                        Console.WriteLine("Errores al crear el usuario: " + errors);
                        TempData["Error"] = errors;
                        ModelState.AddModelError("", errors);
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("Excepción creando usuario: {0}", ex);
                    ModelState.AddModelError("", "Error inesperado al crear el usuario.");
                    return RedirectToAction(nameof(Index));

                }
                var roleName = userVm.Role.ToString();
                if (!await _roleMgr.RoleExistsAsync(roleName))
                    await _roleMgr.CreateAsync(new IdentityRole<int>(roleName));
                await _userMgr.AddToRoleAsync(user, roleName);
                TempData["Message"] = "Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var modelStateErrors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine("Errores en ModelState: " + modelStateErrors);

            }

            TempData["Error"] = "Hubo un problema al crear el usuario. Por favor, verifica los datos ingresados.";
            return RedirectToAction(nameof(Index));
        }

        */
		//Create con SQL directo
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

			// Generar hashes necesarios con el PasswordHasher de Identity
			var dummyUser = new User { Id = 0 }; // sólo para generar el hash
			var passwordHash = _userMgr.PasswordHasher.HashPassword(dummyUser, userVm.Password);
			var securityStamp = Guid.NewGuid().ToString();
			var concurrencyStamp = Guid.NewGuid().ToString();
			var createdAt = DateTime.UtcNow;
			var userType = userVm.Role.ToString();

			using (var tx = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					// 1) Insertar en AspNetUsers
					var insertUserSql = @"
                INSERT INTO AspNetUsers
                    (Name, UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, Role, CreatedAt, UserType, AccessFailedCount, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled)
                VALUES
                    ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13});
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";
					// Obtiene el nuevo Id generado (Identity)
					var newUserId = (await _context.Database.SqlQueryRaw<int>(
						insertUserSql,
						userVm.Name,
						userVm.Name,
						userVm.Email,
						true,
						passwordHash,
						securityStamp,
						concurrencyStamp,
						(int)userVm.Role,
						createdAt,
						userType,
						0,
						false,
						false,
						false)
					.ToListAsync())
					.FirstOrDefault();
					if (newUserId <= 0)
					{
						TempData["Error"] = "No se pudo crear el usuario.";
						await tx.RollbackAsync();
						return RedirectToAction(nameof(Index));
					}

					// 2) Asegurar existencia del rol
					var roleName = userVm.Role.ToString();
					var roleExists = await _context.Roles
						.AnyAsync(r => r.Name == roleName);
					if (!roleExists)
					{
						var insertRoleSql = @"
                    INSERT INTO AspNetRoles (Name, NormalizedName, ConcurrencyStamp)
                    VALUES ({0}, {1}, {2});
                ";
						await _context.Database.ExecuteSqlRawAsync(
							insertRoleSql,
							roleName,
							roleName.ToUpperInvariant(),
							Guid.NewGuid().ToString()
						);
					}

					// 3) Asociar usuario con el rol
					var insertUserRoleSql = @"
                INSERT INTO AspNetUserRoles (UserId, RoleId)
                SELECT u.Id, r.Id
                FROM AspNetUsers u
                JOIN AspNetRoles r ON r.Name = {0}
                WHERE u.Name = {1};
            ";
					var urRows = await _context.Database.ExecuteSqlRawAsync(
						insertUserRoleSql,
						roleName,
						userVm.Name
					);
					if (urRows == 0)
					{
						TempData["Error"] = "No se pudo asignar el rol al usuario.";
						await tx.RollbackAsync();
						return RedirectToAction(nameof(Index));
					}

					await tx.CommitAsync();
					TempData["Message"] = "Usuario creado exitosamente.";
				}
				catch (Exception ex)
				{
					await tx.RollbackAsync();
					_logger.LogError(ex, "Error creando usuario con SQL directo");
					TempData["Error"] = $"Error inesperado al crear el usuario: {ex.Message}";
				}
			}

			return RedirectToAction(nameof(Index));
		}

		/*
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

            user.Name = userVm.Name;
            user.Email = userVm.Email;
            user.UserName = userVm.Email;

            var res = await _userMgr.UpdateAsync(user);
            if (!res.Succeeded)
            {
                TempData["Error"] = string.Join(", ", res.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userMgr.GetRolesAsync(user);
            await _userMgr.RemoveFromRolesAsync(user, currentRoles);

            var newRole = userVm.Role.ToString();
            if (!await _roleMgr.RoleExistsAsync(newRole))
                await _roleMgr.CreateAsync(new IdentityRole<int>(newRole));

            await _userMgr.AddToRoleAsync(user, newRole);
            TempData["Message"] = "Usuario actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        */

		// Método Edit con SQL directo (last resort)

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

			try
			{
				var sql = @"UPDATE AspNetUsers
                SET Name = {0},
                Email = {1},
                Role = {2}
                WHERE Id = {3}";

				int affectedRows = await _context.Database.ExecuteSqlRawAsync(
					sql,
					userVm.Name,
					userVm.Email,
					(int)userVm.Role,
					userVm.Id
				);

				if (affectedRows == 0)
				{
					TempData["Error"] = "No se pudo actualizar el usuario.";
					return RedirectToAction(nameof(Index));
				}

				TempData["Message"] = "Usuario actualizado exitosamente.";
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"Error al actualizar: {ex.Message}";
			}

			return RedirectToAction(nameof(Index));
		}

		/*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id.ToString());
            if (user != null)
            {
                await _userMgr.DeleteAsync(user);
                TempData["Message"] = "Usuario eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        */

		// Delete con SQL directo

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			using (var tx = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					// 1) Quitar asociaciones de roles
					await _context.Database.ExecuteSqlRawAsync(
						"DELETE FROM AspNetUserRoles WHERE UserId = {0};",
						id
					);

					// 2) Eliminar el usuario
					var rows = await _context.Database.ExecuteSqlRawAsync(
						"DELETE FROM AspNetUsers WHERE Id = {0};",
						id
					);

					if (rows == 0)
					{
						TempData["Error"] = "No se encontró el usuario para eliminar.";
						await tx.RollbackAsync();
					}
					else
					{
						await tx.CommitAsync();
						TempData["Message"] = "Usuario eliminado exitosamente.";
					}
				}
				catch (Exception ex)
				{
					await tx.RollbackAsync();
					_logger.LogError(ex, "Error eliminando usuario con SQL directo");
					TempData["Error"] = $"Error al eliminar: {ex.Message}";
				}
			}

			return RedirectToAction(nameof(Index));
		}
		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
