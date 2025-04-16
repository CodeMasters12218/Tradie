using Microsoft.AspNetCore.Mvc;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View("~/Views/Login/Register.cshtml");
        }

        public IActionResult VerifyRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden.");
                    return View("~/Views/Login/Register.cshtml", model);
                }
            }
            return View("~/Views/Login/Register.cshtml", model);
        }
    }
}
