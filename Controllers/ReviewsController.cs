using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Products;
using Microsoft.AspNetCore.Identity;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class ReviewsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public ReviewsController(ApplicationDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Reviews for a product
		public async Task<IActionResult> ProductReviews(int productId)
		{
			var product = await _context.Products
				.Include(p => p.Reviews)
				.ThenInclude(r => r.Customer)
				.FirstOrDefaultAsync(p => p.Id == productId);

			if (product == null)
			{
				return NotFound();
			}

			return View(product.Reviews);
		}

		// GET: Create review form
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Create(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				return NotFound();
			}

			var review = new Review
			{
				ProductId = productId,
				CreatedAt = DateTime.Now
			};

			return View(review);
		}

		// POST: Create review
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Create(Review review)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null || !(user is Customer))
				{
					return Forbid();
				}

				review.CustomerId = user.Id;
				review.CreatedAt = DateTime.Now;

				_context.Add(review);
				await _context.SaveChangesAsync();
				TempData["Message"] = "Reseña creada exitosamente.";
				return RedirectToAction("Details", "Products", new { id = review.ProductId });
			}
			return View(review);
		}

		// GET: Edit review form
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Edit(int id)
		{
			var review = await _context.Reviews.FindAsync(id);
			if (review == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || review.CustomerId != user.Id)
			{
				return Forbid();
			}

			return View(review);
		}

		// POST: Edit review
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> Edit(int id, Review review)
		{
			if (id != review.Id)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || review.CustomerId != user.Id)
			{
				return Forbid();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var existingReview = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
					if (existingReview == null)
					{
						return NotFound();
					}

					// Preserve the original values that shouldn't be changed
					review.CustomerId = existingReview.CustomerId;
					review.ProductId = existingReview.ProductId;
					review.CreatedAt = existingReview.CreatedAt;

					_context.Update(review);
					await _context.SaveChangesAsync();
					TempData["Message"] = "Reseña actualizada exitosamente.";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReviewExists(review.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Details", "Products", new { id = review.ProductId });
			}
			return View(review);
		}

		// GET: Delete confirmation
		[Authorize(Roles = "Customer,Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var review = await _context.Reviews
				.Include(r => r.Product)
				.Include(r => r.Customer)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (review == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || (!(user is Admin) && review.CustomerId != user.Id))
			{
				return Forbid();
			}

			return View(review);
		}

		// POST: Delete review
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Customer,Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var review = await _context.Reviews.FindAsync(id);
			if (review == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || (!(user is Admin) && review.CustomerId != user.Id))
			{
				return Forbid();
			}

			int productId = review.ProductId;
			_context.Reviews.Remove(review);
			await _context.SaveChangesAsync();
			TempData["Message"] = "Reseña eliminada exitosamente.";

			return RedirectToAction("Details", "Products", new { id = productId });
		}

		// POST: Add seller response to review
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Seller")]
		public async Task<IActionResult> AddResponse(int id, string response)
		{
			var review = await _context.Reviews
				.Include(r => r.Product)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (review == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || !(user is Seller seller) || review.Product.SellerId != seller.Id)
			{
				return Forbid();
			}

			review.SellerResponse = response;
			review.ResponseDate = DateTime.Now;

			_context.Update(review);
			await _context.SaveChangesAsync();
			TempData["Message"] = "Respuesta añadida exitosamente.";

			return RedirectToAction("Details", "Products", new { id = review.ProductId });
		}

		private bool ReviewExists(int id)
		{
			return _context.Reviews.Any(e => e.Id == id);
		}
	}
}