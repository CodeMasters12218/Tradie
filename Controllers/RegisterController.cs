using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Tradie.Models.Users;

namespace Tradie.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userMgr;
        private readonly SignInManager<User> _signInMgr;

        public RegisterController(
            UserManager<User> userMgr, SignInManager<User> signInMgr)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Login/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    var user = new User
                    {
                        UserName = model.Name,
                        Email = model.Email
                    };
                    user.Name = model.Name;
                    var createRes = await _userMgr.CreateAsync(user, model.Password);
                    if (!createRes.Succeeded)
                    {
                        foreach (var e in createRes.Errors)
                            ModelState.AddModelError("", e.Description);
                        return View("~/Views/Login/Register.cshtml", model);
                    }
                    await _userMgr.SetUserNameAsync(user, model.Name);

                    await _userMgr.SetEmailAsync(user, model.Email);

                    await _userMgr.AddToRoleAsync(user, "Customer");

                    await _signInMgr.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
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
