using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PustokMVC.DAL;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DashboardController(PustokDbContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        FullName = "Emil Memmedov",
        //        UserName = "emilmemmedov",
        //        Email = "emil@gmail.com"

        //    };
        //    string password = "Emil0077@";
        //    var result = await _userManager.CreateAsync(admin, password);
        //    return Ok(result);
        //}

        //public async Task <IActionResult> CreateRole() 
        //{
        //    IdentityRole role1 = new IdentityRole("SuperAdmin");
        //    IdentityRole role2 = new IdentityRole("Admin");
        //    IdentityRole role3 = new IdentityRole("Member");

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);
        //    return Ok();
        //}

        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser admin = await _userManager.FindByNameAsync("emilmemmedov");

        //    var result = await _userManager.AddToRoleAsync(admin, "Admin");

        //    return Ok(result);
        //}
    }
}
