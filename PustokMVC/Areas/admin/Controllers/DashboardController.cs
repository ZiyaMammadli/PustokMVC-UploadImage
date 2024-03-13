using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PustokMVC.DAL;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    public class DashboardController : Controller
    {
        private readonly PustokDbContext _context;
        public DashboardController(PustokDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
