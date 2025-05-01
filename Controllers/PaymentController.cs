using Microsoft.AspNetCore.Mvc;

namespace Tradie.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult PaymentDetails()
        {
            return View();
        }
    }
}
