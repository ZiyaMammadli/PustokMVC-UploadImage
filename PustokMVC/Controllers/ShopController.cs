using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokMVC.DAL;
using PustokMVC.Models;
using PustokMVC.ViewModels;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PustokMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(PustokDbContext pustokDbContext,UserManager<AppUser> userManager)
        {
            _context = pustokDbContext;
            _userManager = userManager;
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
            AppUser appUser = null;
           var basketVMstr= HttpContext.Request.Cookies["basketVMs"];

            if(!HttpContext.User.Identity.IsAuthenticated)
            {
				if (basketVMstr is not null)
				{
					basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVMstr);

					var basketViewM = basketVMs.FirstOrDefault(bvm => bvm.BookId == bookId);

					if (basketViewM is not null)
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
			}
            else
            {
				string Username = HttpContext.User.Identity.Name;
				appUser = await _userManager.FindByNameAsync(Username);
                var basketItem=await _context.BasketItems.FirstOrDefaultAsync(bi=>bi.AppUserId==appUser.Id && bi.BookId==bookId);
                if (basketItem is not null)
                {
                    basketItem.Count++;                   
                }
                else
                {
                    BasketItem BasketItem = new BasketItem
                    {
                        AppUserId=appUser.Id,
                        BookId=bookId,
                        Count = 1,
                        IsActivated=true,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                    };
                    _context.BasketItems.Add(BasketItem);
                }
            }
            _context.SaveChanges();
            
            return Ok();
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

        public async Task <IActionResult> CheckOut()
        {
            List<CheckOutViewModel> checkOutVMs = new List<CheckOutViewModel>();
            var basketVM = HttpContext.Request.Cookies["basketVMs"];
            double TotalPrice = 0;
            AppUser user = null;
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
				if (basketVM is not null)
				{
					var basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVM);
					foreach (var basketvm in basketVMs)
					{
						Book book = await _context.Books.Where(b => b.Id == basketvm.BookId).FirstOrDefaultAsync();
						CheckOutViewModel checkOutVM = new CheckOutViewModel()
						{
							BookId = basketvm.BookId,
							BookCount = basketvm.Count,
							BookName = book.Name,
							BookPrice = book.SellPrice * basketvm.Count,
						};

						TotalPrice += book.SellPrice * basketvm.Count;
						checkOutVMs.Add(checkOutVM);
					}
				}
			}
            else
            {
                string Username = HttpContext.User.Identity.Name;
                user=await _userManager.FindByNameAsync(Username);
                if (user is not null) 
                { 
                    var basketItems=await _context.BasketItems.Where(bi => bi.AppUserId == user.Id).ToListAsync();
                    if(basketItems is not null)
                    {
                        foreach (var basketItem in basketItems)
                        {
							Book book = await _context.Books.Where(b => b.Id == basketItem.BookId).FirstOrDefaultAsync();
							CheckOutViewModel checkOutVM = new CheckOutViewModel()
                             {
                                 BookId=basketItem.BookId,
                                 BookCount= basketItem.Count,
                                 BookName=book.Name,
                                 BookPrice=book.SellPrice * basketItem.Count,
                             };

							TotalPrice += book.SellPrice * basketItem.Count;
							checkOutVMs.Add(checkOutVM);
                        }
                    }
                }
            }


            ViewData["user"]=user;
            ViewData["TotalPrice"] = TotalPrice;

			return View(checkOutVMs);  
        }
    }
}
