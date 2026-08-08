using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.Auth.Data;
using MVC.Auth.Models;
using System.Security.Claims;

namespace MVC.Auth.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = FakeDbContext.Users
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password."
                );

                return View(model);
            }

            if (user.IsLocked)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account has been locked after 3 failed login attempts."
                );

                return View(model);
            }

            if (user.Password != model.Password)
            {
                user.FailedAttempts++;

                if (user.FailedAttempts >= 3)
                {
                    user.IsLocked = true;

                    ModelState.AddModelError(
                        string.Empty,
                        "Your account has been locked after 3 failed login attempts."
                    );
                }
                else
                {
                    int remainingAttempts = 3 - user.FailedAttempts;

                    ModelState.AddModelError(
                        string.Empty,
                        $"Invalid password. Remaining attempts: {remainingAttempts}"
                    );
                }

                return View(model);
            }

            user.FailedAttempts = 0;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Country, "Philippines"),
                new Claim("Course", "BSIT")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(
            ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = FakeDbContext.Users
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Email address was not found."
                );

                return View(model);
            }

            ViewBag.Message =
                $"Your current password is: {user.Password}";

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(
            ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = FakeDbContext.Users
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "User not found."
                );

                return View(model);
            }

            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Current password is incorrect."
                );

                return View(model);
            }

            user.Password = model.NewPassword;
            user.FailedAttempts = 0;
            user.IsLocked = false;

            ViewBag.Message =
                "Password changed successfully. You can now use your new password.";

            ModelState.Clear();

            return View(new ChangePasswordViewModel());
        }
    }
}