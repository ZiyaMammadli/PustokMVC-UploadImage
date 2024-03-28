using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.DAL;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task <IActionResult> Index()
        {
            return View(await _sliderService.GetAllAsync());
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult>Create(Slider slider)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
               await _sliderService.CreateAsync(slider);
            }
            catch (InvalidContentTypeException ex)
            {

                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(SizeOfFileException ex)
            {
                ModelState.AddModelError(ex.Propertyame, ex.Message);
                return View();  
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Slider Slider=await _sliderService.GetByIdAsync(id);
            if (Slider == null) throw new NotFoundException("This Slider is not found!");
            return View(Slider);  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult>Update(Slider slider)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _sliderService.UpdateAsync(slider);
            }
            catch (NotFoundException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (SizeOfFileException ex)
            {
                ModelState.AddModelError(ex.Propertyame,ex.Message);
                return View();
            }
            catch(InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();  
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task <IActionResult> Delete(int id)
        {
           await _sliderService.DeleteAsync(id);
            return RedirectToAction("index");
        }
    }
}
