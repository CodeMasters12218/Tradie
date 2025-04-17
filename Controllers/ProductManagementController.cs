using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tradie.Data;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
    [Authorize(Roles = "Admin, Seller")]
    public class ProductManagementController : Controller
    {
        private readonly ILogger<ProductManagementController> _logger;
        private readonly ApplicationDbContext _context;

        public ProductManagementController(
            ILogger<ProductManagementController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> ProductRegistry(string searchTerm)
        {
            ViewData["AdminName"] = "Admin X44";
            ViewData["AdminEmail"] = "admin@example.com";

            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query
                    .Where(p => p.Name.Contains(searchTerm)
                             || p.Description.Contains(searchTerm));
            }

            var products = await query
                .Include(p => p.Seller)
                .ToListAsync();

            return View(products);
        }
    }
}
