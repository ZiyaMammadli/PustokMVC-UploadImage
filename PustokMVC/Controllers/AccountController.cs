using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.DAL;
using PustokMVC.Models;
using PustokMVC.ViewModels;

namespace PustokMVC.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager <AppUser> _userManager;
        private readonly PustokDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager <AppUser> userManager,PustokDbContext context,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager; 
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(await _context.AppUsers.AnyAsync(a => a.NormalizedUserName == RegisterVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username already exist");
                return View();
            }
            if(await _context.AppUsers.AnyAsync(a => a.NormalizedEmail == RegisterVM.Email.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This email already exist");
                return View();
            }
            if(await _context.AppUsers.AnyAsync(a => a.PhoneNumber == RegisterVM.PhoneNumber))
            {
                ModelState.AddModelError("UserName", "This phone number already exist");
                return View();
            }
            AppUser member = new AppUser()
            {
                FullName = RegisterVM.UserName,
                UserName = RegisterVM.UserName,
                Email = RegisterVM.Email,
                PhoneNumber = RegisterVM.PhoneNumber,
            };
            string password = RegisterVM.Password;

           var result= await _userManager.CreateAsync(member, password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            var result1 = await _userManager.AddToRoleAsync(member, "Member");

            if(!result1.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(MemberLoginViewModel MemberLoginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser member = null;

            member=await _userManager.FindByNameAsync(MemberLoginVM.UserName);

            if (member is null)
            {
                ModelState.AddModelError("", "Incorrect password or username!");
                return View();  
            }
           var result =await _signInManager.PasswordSignInAsync(member, MemberLoginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect password or username!");
                return View();
            }

            return RedirectToAction("index", "home");
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
