using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Orders;
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
						Orders = new List<string> { "Pedido 1", "Pedido 2" } // Replace with actual logic
					};

					return View(model);
				}
			}

			// If user is not authenticated, redirect to login
			return RedirectToAction("Login", "Account");
		}

		// Simulating data fetching; in real case, you'd fetch by logged-in user ID
		public IActionResult UserEditProfile()
		{
			// Example data, should be retrieved from DB/service
			var model = new UserEditProfileModel
			{
				FullName = "Juan Pérez",
				Email = "juan@example.com",
				PhoneNumber = "123456789",
				Address = "Calle Falsa 123"
			};

			return View(model); // Looks for Views/UserProfile/EditProfile.cshtml
		}

		public IActionResult UserDeleteProfile()
		{
			return View("UserDeleteProfile"); // Looks for Views/UserProfile/UserDeleteProfile.cshtml
		}

		[HttpPost]
		public IActionResult SaveProfile(UserEditProfileModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("EditProfile", model);
			}

			// Save logic here (e.g., database update)

			// Redirect back to main profile page after save
			return RedirectToAction("Index", "UserProfile");
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
                Product = new ProductSummaryDto
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
                Product = new ProductSummaryDto
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
                Product = new ProductSummaryDto
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
                Product = new ProductSummaryDto
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

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteReview(WriteReviewViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

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
            return RedirectToAction(nameof(UserProductReviewsYouWrote), "UserProfile");
        }
    }
}
