﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tradie.Data;
using Tradie.Models.Products;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	//[Authorize(Roles = "Admin, Seller")] por si hace falta revertirlo
	[Authorize]
	public class ProductsController : BaseController
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public ProductsController(
			ApplicationDbContext context,
			UserManager<User> userManager)
			: base(userManager, context)
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

			foreach (var product in products)
			{
				// Random discount from 0% to 50%
				product.DiscountPercentage = new Random().Next(0, 50);
			}

			// Save changes to database
			await _context.SaveChangesAsync();

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

			return View("~/Views/Product/Product.cshtml", product);
		}

		// GET: Products/Create
		[Authorize(Roles = "Admin, Seller")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Seller")]
		public async Task<IActionResult> Create(Product product)
		{
			if (ModelState.IsValid)
			{
				// set time added now
				product.DateAdded = DateTime.UtcNow;

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
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddReview(int productId, int rating, string content)
		{
			// Obtén el ID del usuario autenticado
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

			// Buscar una orden del cliente que incluya este producto
			var order = await _context.Orders
				.Include(o => o.Items)
				.FirstOrDefaultAsync(o => o.CustomerId == userId &&
										  o.Items.Any(i => i.ProductId == productId));

			if (order == null)
			{
				// El usuario no compró este producto → no puede reseñarlo
				TempData["Error"] = "Solo puedes dejar una reseña si has comprado este producto.";
				return RedirectToAction("Details", new { id = productId });
			}

			// Crea la nueva reseña
			var review = new Review
			{
				ProductId = productId,
				CustomerId = userId,
				OrderId = order.Id,
				Rating = rating,
				Content = content,
				CreatedAt = DateTime.UtcNow
			};

			// Añade y guarda la reseña en la base de datos
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();

			// Redirige a la acción que muestra el producto
			return RedirectToAction("Details", new { id = productId });
		}
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> Search(string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return RedirectToAction("Index", "Home");
			}

			var productosFiltrados = await _context.Products
				.Where(p =>
					p.Name.Contains(searchTerm) ||
					p.Description.Contains(searchTerm))
				.OrderBy(p => p.Name)
				.ToListAsync();

			ViewBag.SearchTerm = searchTerm;

			return View("~/Views/Product/SearchResults.cshtml", productosFiltrados);
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}

	}
}
