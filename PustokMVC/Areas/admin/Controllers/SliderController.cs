using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.DAL;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        public SliderController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Index()
        {
           List <Slider> Sliders=await _context.Sliders.ToListAsync();
            return View(Sliders);
        }
        public  IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Slider slider)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Please,You enter jpeg or png file");
                return View();
            }
            if(slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Please,You just can send low measure file from 2 mb!");
                return View();
            }
            string fileName=slider.ImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length-64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = $"C:\\Users\\user\\source\\repos\\PustokMVC\\PustokMVC\\wwwroot\\Uploads\\Sliders\\{fileName}";

            using(FileStream fileStream=new FileStream(path,FileMode.Create))
            {
                slider.ImageFile.CopyTo(fileStream); 
            }

            slider.CreatedDate = DateTime.UtcNow.AddHours(4);
            slider.UpdatedDate = DateTime.UtcNow.AddHours(4);
            slider.ImageUrl = fileName;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Slider Slider=await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (Slider == null) throw new NullReferenceException();
            return View(Slider);  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Update(Slider slider)
        {
            Slider currentSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == slider.Id);
            if (currentSlider == null) throw new NullReferenceException();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Please,You just can send low measure file from 2 mb!");
                return View();
            }
            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Please,You enter only jpeg or png file");
                return View();
            }

            string fileName = slider.ImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = $"C:\\Users\\user\\source\\repos\\PustokMVC\\PustokMVC\\wwwroot\\Uploads\\Sliders\\{fileName}";
            using(FileStream fileStream=new FileStream(path, FileMode.Create))
            {
                slider.ImageFile.CopyTo(fileStream);

            }
            string path2= $"C:\\Users\\user\\source\\repos\\PustokMVC\\PustokMVC\\wwwroot\\Uploads\\Sliders\\{currentSlider.ImageUrl}";

                //if (File.Exists(path2))
                //{
                //    File.Delete(path2);
                //}
            
            
            slider.UpdatedDate = DateTime.UtcNow.AddHours(4);
            currentSlider.ImageUrl=fileName;
            currentSlider.Title1= slider.Title1;
            currentSlider.Title2= slider.Title2;
            currentSlider.Desc=slider.Desc;
            currentSlider.RedirectText=slider.RedirectText;
            currentSlider.RedirectUrl=slider.RedirectUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task <IActionResult> Delete(int id)
        {
           Slider slider= await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == id);
            if(slider == null) throw new NullReferenceException();
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();   
            return RedirectToAction("index");
        }
    }
}
