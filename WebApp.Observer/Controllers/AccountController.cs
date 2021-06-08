using System.Linq;
using System.Threading.Tasks;
using BaseProject.Models;
using BaseProject.Observer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserObserverSubject _userObserverSubject;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserObserverSubject userObserverSubject)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userObserverSubject = userObserverSubject;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
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

        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserCreateViewModel model)
        {
            var appUser = new AppUser {UserName = model.UserName, Email = model.Email};

            var identityResult = await _userManager.CreateAsync(appUser, model.Password);

            if (identityResult.Succeeded)
            {
                _userObserverSubject.NotifyObserver(appUser);
                ViewBag.Message = "Üyelik işlemi başarıyla gerçekleştirildi!";
            }
            else
            {
                ViewBag.message = identityResult.Errors.ToList().First().Description;
            }

            return View();
        }
    }
}