using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Products;
using Tradie.Models.UserCards;
using Tradie.Models.UserProfile;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
	public class UserProfileController : BaseController
	{
		private readonly ApplicationDbContext _context;
		public UserProfileController(UserManager<User> userManager, ApplicationDbContext context)
			: base(userManager, context)
		{
			_context = context;
		}

		public async Task<IActionResult> UserProfileMainPage()
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var user = await GetCurrentUserAsync();

				if (user != null)
				{
					var model = new UserProfileMainPageModel
					{
						FullName = user.Name ?? "No Name",
						Email = user.Email,
						ProfilePhotoUrl = !string.IsNullOrEmpty(user.ProfilePhotoUrl)
					? user.ProfilePhotoUrl
					: "/images/boy_black.png",
						Orders = new List<string> { "Pedido 1", "Pedido 2" }
					};

					return View(model);
				}
			}

			// If user is not authenticated, redirect to login
			return RedirectToAction("Login", "Account");
		}

		public async Task<IActionResult> UserEditProfile()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var model = new UserEditProfileModel
			{
				Name = user.Name,
				LastNames = user.LastNames,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Address = "",
				Country = "",
				City = "",
				Region = "",
				PostalCode = ""
			};

			return View(model);
		}

		public IActionResult UserDeleteProfile()
		{
			return View("UserDeleteProfile");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveProfile(UserEditProfileModel model)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");

			}
			if (!ModelState.IsValid)
			{
				return View("UserEditProfile", model);
			}

			try
			{
				var user = await GetCurrentUserAsync();
				if (user == null)
				{
					ModelState.AddModelError("", "Usuario no encontrado.");
					return View("UserEditProfile", model);
				}

				// Update user properties
				user.Name = model.Name;
				user.LastNames = model.LastNames;
				user.Email = model.Email;
				user.UserName = model.Email;
				user.PhoneNumber = model.PhoneNumber;

				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					// Handle password change if provided
					if (!string.IsNullOrEmpty(model.NewPassword))
					{
						var token = await _userManager.GeneratePasswordResetTokenAsync(user);
						var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

						if (!passwordResult.Succeeded)
						{
							foreach (var error in passwordResult.Errors)
							{
								ModelState.AddModelError("", error.Description);
							}
							return View("UserEditProfile", model);
						}
					}

					TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
					return RedirectToAction("UserProfileMainPage");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Error al actualizar el perfil:" + ex.Message);
			}

			return View("UserEditProfile", model);
		}


		public async Task<IActionResult> UserCards()
		{
			var user = await GetCurrentUserAsync();
			if (user == null)
			{
				ModelState.AddModelError("", "Usuario no encontrado.");
				return View("~/Views/UserProfile/UserCards.cshtml");
			}

			return RedirectToAction("Index", "UserCardProfile", new AdminUserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				LastNames = user.LastNames,
				Role = user.Role,
				Password = user.PasswordHash,
				UserCardProfile = new UserCardProfileModel()
			});

		}

		public async Task<IActionResult> UserProductReviews()
		{
			var userId = Convert.ToInt32(_userManager.GetUserId(User));

			// Buscamos aquellos OrderItems donde el usuario no ha dejado reseña
			var pendingItems = await _context.OrderItems
				.Where(oi => oi.Order.CustomerId == userId && !oi.Order.Reviews.Any(r => r.CustomerId == userId && r.ProductId == oi.ProductId))
				.Include(oi => oi.Order)
				.Include(oi => oi.Product)
				.ToListAsync();
			var model = pendingItems.Select(oi => new PendingReviewViewModel
			{
				OrderId = oi.Order.Id,
				OrderNumber = oi.Order.OrderNumber,
				Product = new Models.Products.ProductSummaryDto
				{
					Id = oi.Product.Id,
					Name = oi.Product.Name,
					Price = oi.Product.Price,
					ImageUrl = oi.Product.ImageUrl
				},
				Quantity = oi.Quantity,
				RatingByUser = oi.Order.Reviews.FirstOrDefault(r => r.CustomerId == userId && r.ProductId == oi.ProductId)?.Rating ?? 0, // Asegura que sea 0 si es nulo
				CommentByUser = oi.Order.Reviews.FirstOrDefault(r => r.CustomerId == userId && r.ProductId == oi.ProductId)?.Content,
				DateByUser = oi.Order.Reviews.FirstOrDefault(r => r.CustomerId == userId && r.ProductId == oi.ProductId)?.CreatedAt ?? DateTime.MinValue
			}).ToList();

			ViewBag.PendingCount = model.Count;
			return View(model ?? new List<PendingReviewViewModel>());
		}


		public async Task<IActionResult> UserProductReviewsAboutYou()
		{
			var userId = Convert.ToInt32(_userManager.GetUserId(User));

			// Ejemplo de búsqueda en BD:
			var reviewsAboutYou = await _context.Reviews
				.Where(r => r.SellerId == userId && r.SellerResponse == null || r.SellerResponse != null)
				.Include(r => r.Order)
					.ThenInclude(o => o.Items)
						.ThenInclude(i => i.Product)
				.Include(r => r.Customer) // Cliente que te valora
				.ToListAsync();

			var model = reviewsAboutYou.Select(r => new ReviewAboutYouViewModel
			{
				OrderNumber = r.Order.OrderNumber,
				Product = new Models.Products.ProductSummaryDto
				{
					Id = r.Product.Id,
					Name = r.Product.Name,
					Price = r.Product.Price,
					ImageUrl = r.Product.ImageUrl
				},
				Quantity = r.Order.Items.First(i => i.ProductId == r.ProductId).Quantity,
				ReviewerName = r.Customer.Name,
				ReviewerRating = r.Rating,
				ReviewerComment = r.Content,
				ReviewerDate = r.CreatedAt,
				SellerRating = r.SellerRating,
				SellerComment = r.SellerResponse,
				SellerDate = r.ResponseDate
			}).ToList();

            // Contar valoraciones pendientes (aquellas que aún no tienen valoración del usuario)
            var pendingCount = await _context.Orders
                .Where(o => o.CustomerId == userId &&
                            !_context.Reviews.Any(r => r.OrderId == o.Id))
                .CountAsync();

            ViewBag.PendingCount = pendingCount;


            return View(model);
		}


		public async Task<IActionResult> UserProductReviewsYouWrote()
		{
			var userId = Convert.ToInt32(_userManager.GetUserId(User));

			var reviews = await _context.Reviews
				.Where(r => r.CustomerId == userId)
				.Include(r => r.Order)
				.Include(r => r.Product)
				.ToListAsync();

			var model = reviews.Select(r => new UserReviewViewModel
			{
				OrderNumber = r.Order.OrderNumber,
				Product = new Models.Users.ProductSummaryDto
				{
					Id = r.Product.Id,
					Name = r.Product.Name,
					Price = r.Product.Price,
					ImageUrl = r.Product.ImageUrl
				},
				RatingBySeller = r.Rating,
				CommentBySeller = r.Content,
				DateBySeller = r.CreatedAt
			}).ToList();

			return View(model);
		}


		public IActionResult UserProductReviewsWrite()
		{
			return View();
		}

		public async Task<IActionResult> WriteReview(int orderId, int productId)
		{
			// Buscar datos de pedido/producto en base de datos
			var order = await _context.Orders
								 .Include(o => o.Items)
								 .ThenInclude(i => i.Product)
								 .FirstOrDefaultAsync(o => o.Id == orderId);
			var item = order.Items.First(i => i.ProductId == productId);

			var vm = new WriteReviewViewModel
			{
				OrderId = order.Id,
				OrderNumber = order.Id.ToString(),
				Product = new Models.Products.ProductSummaryDto
				{
					Id = item.Product.Id,
					Name = item.Product.Name,
					Price = item.Product.Price,
					ImageUrl = item.Product.ImageUrl
				},
				Quantity = item.Quantity,
				UserRating = 0,
				UserComment = string.Empty
			};

			return View("~/Views/UserProfile/UserProductReviewsWrite.cshtml", vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> WriteReview(WriteReviewViewModel vm)
		{
			// Crear entidad Review en BD basándose en vm.OrderId, vm.Product.Id, vm.UserRating, vm.UserComment, UsuarioActual
			var review = new Review
			{
				OrderId = vm.OrderId,
				ProductId = vm.Product.Id,
				CustomerId = Convert.ToInt32(_userManager.GetUserId(User)),
				Rating = vm.UserRating,
				Content = vm.UserComment,
				CreatedAt = DateTime.UtcNow
			};
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();

			// Redirigir a la lista de "Tus valoraciones", por ejemplo:
			return RedirectToAction("UserProductReviewsYouWrote", "UserProfile");
		}
	}
}
