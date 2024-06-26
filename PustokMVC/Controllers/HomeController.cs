using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.DAL;
using PustokMVC.ViewModels;
using System.Diagnostics;

namespace PustokMVC.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly PustokDbContext _context;
        public HomeController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Sliders= await _context.Sliders.ToListAsync(),
                Books= await _context.Books.Include(b=>b.Author).Include(b=>b.BookImages).Include(b=>b.Genre).ToListAsync(),
            };
            return View(homeViewModel);
        }
    }
}
