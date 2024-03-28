using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokMVC.Areas.admin.ViewModels;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    public class AuthController : Controller
    {
        private readonly UserManager <AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager <AppUser> userManager,SignInManager <AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(AdminViewModel AdminVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser admin = null;

            admin= await _userManager.FindByNameAsync(AdminVM.UserName);

            if(admin is null)
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View();
            }

           var result= await _signInManager.PasswordSignInAsync(admin,AdminVM.Password,false,false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View();
            }

            return RedirectToAction("Index","Dashboard");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
