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
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;
        
        public BookController(IBookService bookService,IGenreService genreService,IAuthorService authorService)
        {
            _bookService = bookService;
            _genreService = genreService;
            _authorService = authorService;
        }
        public async Task <IActionResult> Index()
        {  
            return View(await _bookService.GetAllAsync(null, "Genre", "Author", "BookImages"));
        }
        public  async Task <IActionResult> Create()
        {
            ViewData["genres"]=await _genreService.GetAllAsync();
            ViewData["authors"]=await _authorService.GetAllAsync(); 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task <IActionResult> Create(Book book)
        {
            ViewData["genres"] = await _genreService.GetAllAsync();
            ViewData["authors"] = await _authorService.GetAllAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _bookService.CreateAsync(book);
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
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("Index");
        }
        public async Task <IActionResult>Update(int id)
        {
            ViewData["genres"] = await _genreService.GetAllAsync();
            ViewData["authors"] = await _authorService.GetAllAsync();
            Book? book = null;
            try
            {
                book = await _bookService.GetSingleAsync(b => b.Id == id, "Author", "Genre","BookImages");
            }
            catch (Exception)
            {

                throw;
            }
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task <IActionResult> Update(Book book)
        {
            ViewData["genres"] = await _genreService.GetAllAsync();
            ViewData["authors"] = await _authorService.GetAllAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _bookService.UpdateAsync(book);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (NameAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (SizeOfFileException ex)
            {
                ModelState.AddModelError(ex.Propertyame, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task <IActionResult>Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction("index");
        }
    }
}
