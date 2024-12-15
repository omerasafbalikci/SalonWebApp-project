using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SalonWebApp.Models;

namespace SalonWebApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly SalonContext context;

        public MemberController(SalonContext context)
        {
            this.context = context;
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var user = this.context.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user != null && user.Password == model.OldPassword)
                {
                    user.Password = model.NewPassword;
                    this.context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found or old password is incorrect.");
                }
            }
            return View(model);
        }

        public IActionResult Login() 
        {
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user, string returnUrl)
        {
            var kullanici = this.context.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            if (kullanici != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
