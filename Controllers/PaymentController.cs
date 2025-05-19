using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tradie.Models.Payments;
using Tradie.Models.Users;


namespace Tradie.Controllers
{
    public class PaymentController : BaseController
	{
		// Constructor that accepts UserManager<User> and passes it to the BaseController constructor
		public PaymentController(
			UserManager<User> userManager
		) : base(userManager)  // Passing UserManager to the BaseController constructor
		{
		}

		public IActionResult PaymentDetails()
        {
            var model = new PaymentDetails();
			return View();
        }

		public IActionResult PaymentMethod()
		{
			var model = new PaymentMethod();
			return View();
		}

		public IActionResult PaymentSummary()
		{
			var model = new PaymentSummary();
			return View();
		}
	}
}
