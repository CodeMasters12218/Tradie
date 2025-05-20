using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Products;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
    [Authorize(Roles = "Admin, Seller")]
    public class ProductsController : BaseController
	{
        private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public ProductsController(
			ApplicationDbContext context, 
			UserManager<User> userManager)
			: base(userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(string searchTerm, string subcategory)
        {
			var productsQuery = _context.Products
				.Include(p => p.Seller)
				.AsQueryable();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				productsQuery = productsQuery.Where(p =>
					p.Name.Contains(searchTerm) ||
					p.Description.Contains(searchTerm));
			}

			if (!string.IsNullOrEmpty(subcategory))
			{
				productsQuery = productsQuery.Where(p => p.Subcategory == subcategory);
			}

			var products = await productsQuery.ToListAsync();
            return View(products);
        }

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products
				.Include(p => p.Seller)
				.Include(p => p.Reviews)
					.ThenInclude(r => r.Customer)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (product == null) return NotFound();

			// Check if current user has already reviewed this product
			bool hasReviewed = false;
			if (User?.Identity != null && User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				hasReviewed = product.Reviews.Any(r => r.Customer != null && r.Customer.Id.ToString() == userId);
				ViewData["HasReviewed"] = hasReviewed;
			}

			return View(product);
		}

		// GET: Products/Create
		[Authorize(Roles = "Admin, Seller")]
        public IActionResult Create()
        {
            return View();
        }

		// POST: Products/Create
		// POST: Products/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> Create(Product product)
		{
			if (ModelState.IsValid)
			{
				// Set the seller ID to the current user if they're a seller
				var user = await _userManager.GetUserAsync(User);
				if (user is Seller seller)
				{
					product.SellerId = seller.Id;
				}
				else if (User.IsInRole("Admin"))
				{
					// If admin is creating a product, needs to specify SellerId
					if (product.SellerId == 0)
					{
						ModelState.AddModelError("SellerId", "El ID del vendedor es requerido cuando un administrador crea un producto.");
						return View(product);
					}
				}

				_context.Add(product);
				await _context.SaveChangesAsync();
				TempData["Message"] = "Producto creado exitosamente.";
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// GET: Products/Edit/5
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			// Check if the current user is the seller of this product or an admin
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Forbid();
			}

			if (!(user is Admin) && product.SellerId != user.Id)
			{
				return Forbid();
			}

			return View(product);
		}

		// POST: Products/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> Edit(int id, Product product)
		{
			if (id != product.Id) return NotFound();

			// Verify user is allowed to edit this product
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Forbid();
			}

			var originalProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

			if (originalProduct == null)
			{
				return NotFound();
			}

			if (!(user is Admin) && originalProduct.SellerId != user.Id)
			{
				return Forbid();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Maintain the original seller ID
					product.SellerId = originalProduct.SellerId;

					_context.Update(product);
					await _context.SaveChangesAsync();
					TempData["Message"] = "Producto actualizado exitosamente.";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.Id))
						return NotFound();
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// GET: Products/Delete/5
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var product = await _context.Products
				.Include(p => p.Seller)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (product == null) return NotFound();

			// Check if the current user is the seller of this product or an admin
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return Forbid();
			}

			if (!(user is Admin) && product.SellerId != user.Id)
			{
				return Forbid();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			// Check if the current user is the seller of this product or an admin
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Forbid();
			}

			if (!(user is Admin) && product.SellerId != user.Id)
			{
				return Forbid();
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			TempData["Message"] = "Producto eliminado exitosamente.";

			return RedirectToAction(nameof(Index));
		}


		private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

	}
}
