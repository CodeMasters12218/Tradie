using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }


        [HttpPost]
        public IActionResult VerifyLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password))
                {
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", "El correo o la contraseña son incorrectos.");
                    return View("~/Views/Login/Login.cshtml", model);
                }
            }
            return View("~/Views/Login/Login.cshtml", model);
        }
    }
}
