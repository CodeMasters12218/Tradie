using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;

namespace Tradie.Controllers
{
    public class UserDeleteProfileController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userMgr;
        private readonly RoleManager<IdentityRole<int>> _roleMgr;
        private readonly ILogger<UsersController> _logger;

        public UserDeleteProfileController(UserManager<User> userManager, ApplicationDbContext context, UserManager<User> userMgr, RoleManager<IdentityRole<int>> roleMgr, ILogger<UsersController> logger)
            : base(userManager, context)
        {
            _context = context;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            var user = await GetCurrentUserAsync();

            using (var tx = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1) Quitar asociaciones de roles
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM AspNetUserRoles WHERE UserId = {0};",
                        user.Id
                    );

                    // 2) Eliminar el usuario
                    var rows = await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM AspNetUsers WHERE Id = {0};",
                        user.Id
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

            return RedirectToAction("Login", "Account");
        }
    }
}
