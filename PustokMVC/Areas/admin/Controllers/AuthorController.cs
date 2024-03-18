using Microsoft.AspNetCore.Mvc;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.Models;

namespace PustokMVC.Areas.admin.Controllers
{
    [Area("admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task <IActionResult> Index()
        {
            return View(await _authorService.GetAllAsync());
        }
        public  IActionResult Create()
        {            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _authorService.Create(author);
            }
            catch (NameAlreadyExistException ex)
            {

                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task <IActionResult> Update(int id)
        {
            return View(await _authorService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult>Update(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _authorService.Update(author);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (NameAlreadyExistException ex)
            {

                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }       
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task <IActionResult> Delete(int id)
        {
            try
            {
                await _authorService.Delete(id);
            }
            catch (NotFoundException)
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
