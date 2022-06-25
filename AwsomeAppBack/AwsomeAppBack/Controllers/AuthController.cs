using AwsomeAppBack.DAL;
using AwsomeAppBack.Models;
using AwsomeAppBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AwsomeAppBack.Controllers
{
    public class AuthController : Controller
    {
        readonly UserManager<AppUser> _usermanager;
        readonly SignInManager<AppUser> _signInManager;
        public AuthController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _usermanager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill all Inputs");
                return View(loginVM);
            }
            AppUser user = null;
            if (loginVM.Login.Contains("@"))
                user = await _usermanager.FindByEmailAsync(loginVM.Login);

            else
                user = await _usermanager.FindByNameAsync(loginVM.Login);

            if (user == null)
            {
                ModelState.AddModelError("", "Wrong Login/Password"); ;
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Locked");
                    return View(loginVM);
                }

                ModelState.AddModelError("", "Wrong Login/Password");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill all Inputs");
                return View(registerVM);
            }
            AppUser user = new AppUser()
            {
                UserName = registerVM.Login,
                Email = registerVM.Email

            };
            var result = await _usermanager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ToString());
                    return View(registerVM);
                }
            }

            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
