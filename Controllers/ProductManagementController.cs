using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public ProductManagementController(
			ILogger<ProductManagementController> logger,
			ApplicationDbContext context, UserManager<User> userMgr, IMemoryCache cache)
		{
			_logger = logger;
			_context = context;
			_userMgr = userMgr;
			_cache = cache;
		}

		[HttpGet]
		public async Task<IActionResult> ProductRegistry(string? searchTerm, string? category, int page = 1, int pageSize = 20)
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

            var query = _context.Products
				.AsNoTracking()
				.Select(p => new ProductSummaryDto
				{
					Id = p.Id,
					Name = p.Name,
					Category = p.Category,
					Subcategory = p.Subcategory,
					Price = p.Price,
					Discount = p.DiscountPercentage,
					Stock = p.Stock,
					ImageUrl = p.ImageUrl
				});


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

            var totalProducts = await _context.Products
				.Where(p =>
					(string.IsNullOrEmpty(searchTerm) || p.Name.Contains(searchTerm)) &&
					(string.IsNullOrEmpty(category) || (p.Subcategory != null && p.Subcategory.ToLower() == category.ToLower()))
				)
				.CountAsync();


            query = query
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

            var categories = await _cache.GetOrCreateAsync("AllCategories", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30); // tiempo que dure el cache
                return await _context.Categories.AsNoTracking().ToListAsync();
            });

            var vm = new ProductRegistryViewModel
			{
				Products = await query.ToListAsync(),
				SearchTerm = searchTerm,
				Categories = categories,
				Sellers = sellers,
				CurrentPage = page,
				TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
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
                .AsNoTracking()
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Subcategory = p.Subcategory,
                    Price = p.Price,
                    Discount = p.DiscountPercentage,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl
                })
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
                .AsNoTracking()
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Subcategory = p.Subcategory,
                    Price = p.Price,
                    Discount = p.DiscountPercentage,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl
                })
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
