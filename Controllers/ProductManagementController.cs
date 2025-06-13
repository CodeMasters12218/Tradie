using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Products;

namespace Tradie.Controllers
{
	[Authorize(Roles = "Admin, Seller")]
	public class ProductManagementController : Controller
	{
		private readonly ILogger<ProductManagementController> _logger;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userMgr;

		public ProductManagementController(
			ILogger<ProductManagementController> logger,
			ApplicationDbContext context, UserManager<User> userMgr)
		{
			_logger = logger;
			_context = context;
			_userMgr = userMgr;
		}

		[HttpGet]
		public async Task<IActionResult> ProductRegistry(string? searchTerm, string? category)
		{
			// For logged in admin 
			var currentAdmin = await _userMgr.GetUserAsync(User);

			IEnumerable<User>? sellers = null;
			if (await _userMgr.IsInRoleAsync(currentAdmin, "Admin"))
			{
				sellers = await _userMgr.GetUsersInRoleAsync("Seller");
			}

			if (currentAdmin != null)
			{
				ViewData["AdminName"] = currentAdmin.Name;
				ViewData["AdminEmail"] = currentAdmin.Email;
				ViewData["AdminPhoto"] = currentAdmin.ProfilePhotoUrl;
			}
			else
			{
				// Optional fallback
				ViewData["AdminName"] = "Admin";
				ViewData["AdminEmail"] = "admin@example.com";
				ViewData["AdminPhoto"] = null;
			}

			var query = _context.Products.Include(p => p.Seller).AsQueryable();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query
					.Where(p => p.Name.Contains(searchTerm)
							 || p.Description.Contains(searchTerm));
			}

			if (!string.IsNullOrEmpty(category))
			{
				query = query.Where(p => p.Subcategory != null &&
										p.Subcategory.ToLower() == category.ToLower());
			}

			var vm = new ProductRegistryViewModel
			{
				Products = await query.ToListAsync(),
				SearchTerm = searchTerm,
				Categories = await _context.Categories.ToListAsync(),
				Sellers = sellers
			};

			return View(vm);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ProductRegistry(ProductRegistryViewModel vm)
		{
			var product = vm.NewProduct;
			var currentUser = await _userMgr.GetUserAsync(User);
            var isAdmin = await _userMgr.IsInRoleAsync(currentUser, "Admin");
            var isSeller = await _userMgr.IsInRoleAsync(currentUser, "Seller");

			product.Category = null;

			if (isSeller)
			{
				product.SellerId = currentUser.Id;
			}
			else if (isAdmin)
			{
				if (product.SellerId == 0)
				{
                    ModelState.AddModelError("NewProduct.SellerId", "Debes seleccionar un vendedor.");
                }
			}

			if (ModelState.IsValid)
			{
				_context.Products.Add(product);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(ProductRegistry));
			}

			vm.Products = await _context.Products
				.Include(p => p.Seller)
				.Include(p => p.Category)
				.ToListAsync();

			vm.Categories = await _context.Categories.ToListAsync();
            if (isAdmin)
            {
                vm.Sellers = await _userMgr.GetUsersInRoleAsync("Seller");
            }

            return View(vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProduct(ProductRegistryViewModel vm)
		{
			var prod = vm.NewProduct;
			var user = await _userMgr.GetUserAsync(User);
            var isAdmin = await _userMgr.IsInRoleAsync(user, "Admin");
            var isSeller = await _userMgr.IsInRoleAsync(user, "Seller");

			if (isSeller)
			{
				prod.SellerId = user.Id;
			}

			prod.Category = null;

			if (ModelState.IsValid)
			{
				_context.Products.Update(prod);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(ProductRegistry));
			}
			vm.Products = await _context.Products
				 .Include(p => p.Seller)
				 .Include(p => p.Category)
				 .ToListAsync();

			vm.Categories = await _context.Categories.ToListAsync();

			if (isAdmin)
			{
				vm.Sellers = await _userMgr.GetUsersInRoleAsync("Seller");
            }

			return View("ProductRegistry", vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var prod = await _context.Products.FindAsync(id);
			if (prod != null)
			{
				_context.Products.Remove(prod);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(ProductRegistry));
		}


	}
}
