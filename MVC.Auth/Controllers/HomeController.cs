using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Auth.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MVC.Auth.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var country = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Country)?
                .Value;

            ViewBag.Country = country;

            return View();
        }

        public IActionResult Privacy()
        {
            var user = HttpContext.User.Identity;

            ViewBag.WelcomeScript =
                $"Welcome back to the Philippines, {user?.Name}!";

            
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}