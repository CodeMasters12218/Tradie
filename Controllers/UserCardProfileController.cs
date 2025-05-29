using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.UserCards;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
    public class UserCardProfileController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private object _logger;

        public UserCardProfileController(UserManager<User> userManager, ApplicationDbContext context)
            : base(userManager, context)
        {
            _context = context;
        }
        public IActionResult Index(AdminUserViewModel user)
        {
            int userID = user.Id;
            var cards = _context.UserCards.Where(card => card.UserId == user.Id).ToList();

			AdminUserViewModel model = new AdminUserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				LastNames = user.LastNames,
				Role = user.Role,
				UserCardProfile = new UserCardProfileModel
				{
					Cards = cards,
					CurrentCard = cards.FirstOrDefault()
				}
			};

			return View("~/Views/UserProfile/UserCards.cshtml", model);
		}


		[HttpPost]
		public async Task<IActionResult> CreateCard(UserCardModel model)
		{
			var user = await GetCurrentUserAsync();
			if (model == null)
			{
				ModelState.AddModelError("", "No se recibieron datos para la tarjeta.");
				return View("~/Views/UserProfile/UserCards.cshtml");
			}

			if (!string.IsNullOrEmpty(model.Email))
			{
				model.UserId = user.Id;
				_context.UserCards.Add(model);
				_context.SaveChanges();

				AdminUserViewModel AdminUser2 = new AdminUserViewModel
				{
					Id = user.Id,
					Name = user.Name,
					Email = user.Email,
					LastNames = user.LastNames,
					Role = user.Role,
					Password = user.PasswordHash,
					UserCardProfile = new UserCardProfileModel
					{
						Cards = _context.UserCards.Where(card => card.UserId == user.Id).ToList(),
						CurrentCard = model
					}
				};

				return View("~/Views/UserProfile/UserCards.cshtml", AdminUser2);
			}

			if (string.IsNullOrEmpty(model.Payeer) || string.IsNullOrEmpty(model.Number) || model.ExpiryDate == default || string.IsNullOrEmpty(model.Cvv))
			{
				ModelState.AddModelError("", "Todos los campos son obligatorios.");
				return View("~/Views/UserProfile/UserCreditCardCreate.cshtml");
			}

			model.UserId = user.Id;
			_context.UserCards.Add(model);
			_context.SaveChanges();

			AdminUserViewModel AdminUser = new AdminUserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				LastNames = user.LastNames,
				Role = user.Role,
				Password = user.PasswordHash,
				UserCardProfile = new UserCardProfileModel
				{
					Cards = _context.UserCards.Where(card => card.UserId == user.Id).ToList(),
					CurrentCard = model
				}
			};

			return View("~/Views/UserProfile/UserCards.cshtml", AdminUser);
		}

        public object Get_logger()
        {
            return _logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCard(int cardId, object _logger)
        {
            using (var tx = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verificar si la tarjeta existe antes de eliminarla
                    var cardExists = await _context.UserCards.AnyAsync(c => c.Id == cardId);
                    if (!cardExists)
                    {
                        TempData["Error"] = "No se encontró la tarjeta de crédito para eliminar.";
                        return RedirectToAction(nameof(Index));
                    }

					// Eliminar la tarjeta de crédito seleccionada
					var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
						"DELETE FROM UserCards WHERE Id = {0};",
						cardId
					);

                    if (rowsAffected == 0)
                    {
                        TempData["Error"] = "No se pudo eliminar la tarjeta.";
                        await tx.RollbackAsync();
                    }
                    else
                    {
                        await tx.CommitAsync();
                        TempData["Message"] = "Tarjeta eliminada exitosamente.";
                    }
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    TempData["Error"] = $"Error al eliminar tarjeta: {ex.Message}";
                }
            }

			return RedirectToAction(nameof(Index));
		}


		public IActionResult UserCardCreate()
		{
			return View("~/Views/UserProfile/UserCreditCardCreate.cshtml");
		}

		public IActionResult UserCardCreatePaypal()
		{
			return View("~/Views/UserProfile/UserCardCreatePaypal.cshtml");
		}

		public IActionResult UserGooglePayCard()
		{
			return View("~/Views/UserProfile/UserGooglePayCard.cshtml");
		}
	}
}
