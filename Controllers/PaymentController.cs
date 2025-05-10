using Microsoft.AspNetCore.Mvc;
using Tradie.Models.Payments;


namespace Tradie.Controllers
{
    public class PaymentController : Controller
    {
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
