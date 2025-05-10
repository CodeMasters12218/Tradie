using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                usersQuery = usersQuery.Where(u =>
                    u.Name.Contains(searchTerm!) ||
                    u.Email.Contains(searchTerm!));
            var users = await usersQuery.ToListAsync();

            var userViewModels = new List<AdminUserViewModel>();
            foreach (var user in users)
            {
                var roleName = (await _userMgr.GetRolesAsync(user)).FirstOrDefault();
                userViewModels.Add(new AdminUserViewModel
                {

                    Name = user.Name,
                    Role = roleName switch
                    {
                        "Admin" => UserRole.Admin,
                        "Seller" => UserRole.Seller,
                        _ => UserRole.Customer
                    },
                });
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
        [HttpGet]
        public IActionResult Create()
            => View(new AdminUserViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = vm.Name,
                    Role = vm.Role,
                    EmailConfirmed = true

                };
                try
                {

                    Console.WriteLine(">>> Entrando a CreateAsync");
                    var result = await _userMgr.CreateAsync(user, vm.Password);
                    Console.WriteLine(">>> Resultado: " + result.Succeeded);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                        Console.WriteLine("Errores al crear usuario: " + errors);
                        ModelState.AddModelError("", errors);
                        return View(vm);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("Excepción creando usuario: {0}", ex);
                    ModelState.AddModelError("", "Error inesperado al crear el usuario.");
                    return View(vm);

                }    
                var roleName = vm.Role.ToString();
                if (!await _roleMgr.RoleExistsAsync(roleName))
                    await _roleMgr.CreateAsync(new IdentityRole<int>(roleName));
                await _userMgr.AddToRoleAsync(user, roleName);
                TempData["Message"] = "Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            } else
            {
                var modelStateErrors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine("Errores en ModelState: " + modelStateErrors);
            }

            TempData["Error"] = "Hubo un problema al crear el usuario. Por favor, verifica los datos ingresados.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {

            var user = await _userMgr.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            var roles = await _userMgr.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault();

            var roleEnum = UserRole.Customer;
            if (!string.IsNullOrEmpty(roleName))
            {
                Enum.TryParse<UserRole>(roleName, out roleEnum);
            }
            var vm = new AdminUserViewModel
            {
                Name = user.Name,
                Role = roleEnum
            };
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminUserViewModel vm)
        {

            if (ModelState.IsValid)
            {

                int id = 0;
                var user = await _userMgr.FindByIdAsync(id.ToString());
                user.Name = vm.Name;
                var res = await _userMgr.UpdateAsync(user);
                if (!res.Succeeded)
                {
                    foreach (var e in res.Errors)
                        ModelState.AddModelError("", e.Description);
                    return View(vm);
                }
                var currentRoles = await _userMgr.GetRolesAsync(user);
                await _userMgr.RemoveFromRolesAsync(user, currentRoles);
                var roleName = vm.Role.ToString();
                if (!await _roleMgr.RoleExistsAsync(roleName))
                    await _roleMgr.CreateAsync(new IdentityRole<int>(roleName));
                await _userMgr.AddToRoleAsync(user, roleName);

                TempData["Message"] = "Usuario actualizado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
