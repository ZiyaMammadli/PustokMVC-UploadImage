using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokMVC.DAL;
using PustokMVC.ViewModels;
using System.Text.Json.Serialization;

namespace PustokMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly PustokDbContext _context;

        public ShopController(PustokDbContext pustokDbContext)
        {
            _context = pustokDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> AddToBasket(int bookId)
        {
           if(! await _context.Books.AnyAsync(b=>b.Id == bookId)) return NotFound();

            List<BasketViewModel> basketVMs = new List<BasketViewModel>();
            BasketViewModel basketVM = null;
           var basketVMstr= HttpContext.Request.Cookies["basketVMs"];

            if(basketVMstr is not null)
            {
                 basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVMstr);

                var basketViewM = basketVMs.FirstOrDefault(bvm => bvm.BookId == bookId);

                if(basketViewM is not null)
                {
                    basketViewM.Count++;
                    
                }
                else
                {
                    basketVM = new BasketViewModel
                    {
                        BookId = bookId,
                        Count = 1
                    };
                    basketVMs.Add(basketVM);
                }              
            }
            else
            {
                basketVM = new BasketViewModel
                {
                    BookId = bookId,
                    Count = 1
                };
                basketVMs.Add(basketVM);
            }

            var BasketVMstr = JsonConvert.SerializeObject(basketVMs);

            HttpContext.Response.Cookies.Append("basketVMs", BasketVMstr);

            return RedirectToAction("index","home");
        }

        public IActionResult GetBasket()
        {
            List<BasketViewModel> BasketVMs = new List<BasketViewModel>();

            var basketVMs = HttpContext.Request.Cookies["BasketVMs"];
            if(basketVMs is not null)
            {
                BasketVMs=JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVMs);
            }


            return Ok(BasketVMs);
        }

        public async Task <IActionResult> RemoveBasket(int bookId)
        {
            if (!await _context.Books.AnyAsync(b => b.Id == bookId)) return NotFound();

            List<BasketViewModel> BasketVMs = new List<BasketViewModel>();

            var basketVM = HttpContext.Request.Cookies["basketVMs"];

            if(basketVM is not null) 
            { 
                BasketVMs=JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVM);

                var BasketVM2= BasketVMs.Where(bvm => bvm.Count == 1 && bvm.BookId == bookId).FirstOrDefault();
                if(BasketVM2 is not null)
                {
                    BasketVMs.Remove(BasketVM2);
                }
                var BasketVM=BasketVMs.Where(bvm=>bvm.Count>1 && bvm.BookId==bookId).FirstOrDefault();
                if(BasketVM is not null)
                {
                    BasketVM.Count--;
                }
            }
            var BasketVMstr = JsonConvert.SerializeObject(BasketVMs);

            HttpContext.Response.Cookies.Append("basketVMs", BasketVMstr);

            return RedirectToAction("index", "home");
        }
    }
}
