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
        public async Task<IActionResult> DeleteCard(int cardId)
        {
            try
            {
                var card = await _context.UserCards.FirstOrDefaultAsync(c => c.Id == cardId);
                if (card == null)
                {
                    TempData["Error"] = "No se encontró la tarjeta de crédito para eliminar.";
                    return RedirectToAction(nameof(Index));
                }

                _context.UserCards.Remove(card);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Tarjeta eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar tarjeta: {ex.Message}";
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
