using System.Threading.Tasks;
using WebApp.Strategy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Strategy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var hasUser = await _userManager.FindByEmailAsync(Email);

            if (hasUser == null) return View();

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, Password, true,false);

            if (!signInResult.Succeeded) return View();

            return RedirectToAction(nameof(HomeController.Index),"Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }
    }
}