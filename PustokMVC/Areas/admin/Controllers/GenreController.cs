using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;   
        public GenreController(IGenreService GenreService)
        {
            _genreService = GenreService;
        }
        public async Task <IActionResult> Index()
        {
            
            return View(await _genreService.GetAllAsync());
        }
        public   IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task <IActionResult> Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _genreService.CreateAsync(genre);
            }           
            catch (NameAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task <IActionResult>Update(int id)
        {
            
            return View(await _genreService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task <IActionResult> Update(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _genreService.UpdateAsync(genre);
            }
            catch (NameAlreadyExistException ex)
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
            try
            {
                await _genreService.DeleteAsync(id);
            }
            catch (NotFoundException )
            {
                return NotFound();
            }
            catch (Exception )
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
