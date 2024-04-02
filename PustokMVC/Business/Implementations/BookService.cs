using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.BookExceptions;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.DAL;
using PustokMVC.Models;
using System.Globalization;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations
{
    public class BookService : IBookService
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BookService(PustokDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task CreateAsync(Book book)
        {
            if (book.CoverImageFile is not null)
            {
                if (book.CoverImageFile.ContentType != "image/jpeg" && book.CoverImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
                }
                if (book.CoverImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = book.CoverImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.CoverImageFile.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    ImageUrl = fileName,
                    IsCover = true,
                };
                await _context.BookImages.AddAsync(bookImage);
            }


            if (book.HoverImageFile is not null)
            {
                if (book.HoverImageFile.ContentType != "image/jpeg" && book.HoverImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("HoverImageFile", "Please,You enter jpeg or png file");
                }
                if (book.HoverImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("HoverImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = book.HoverImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.HoverImageFile.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    ImageUrl = fileName,
                    IsCover = false,
                };
                await _context.BookImages.AddAsync(bookImage);
            }


            if (book.ImageFiles is not null)
            {
                foreach (var ImageFile in book.ImageFiles)
                {
                    if (ImageFile.ContentType != "image/jpeg" && ImageFile.ContentType != "image/png")
                    {
                        throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                    }
                    if (ImageFile.Length > 2097152)
                    {
                        throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                    }
                    string fileName = ImageFile.FileName;
                    if (fileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }
                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }

                    BookImage bookImage = new BookImage()
                    {
                        Book = book,
                        ImageUrl = fileName,
                        IsCover = null,
                    };
                    await _context.BookImages.AddAsync(bookImage);
                }

            }


            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Book? deletedBook= await _context.Books.FirstOrDefaultAsync(b=>b.Id == id);
            if ( deletedBook is null) throw new NotFoundException("Book is not found!");

            
            if (deletedBook.CoverImageFile is not null)
            {
              
                BookImage? coverImage = await _context.BookImages.Where(bi => bi.IsCover == true).Where(bi => bi.BookId == deletedBook.Id).FirstOrDefaultAsync();

                
                
                    string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", coverImage.ImageUrl);
                    if (File.Exists(path2))
                    {
                        File.Delete(path2);
                    }
                    _context.BookImages.Remove(coverImage);
                              
       
            }
            if(deletedBook.HoverImageFile is not null) 
            {
                BookImage? hoverImage = await _context.BookImages.Where(bi => bi.IsCover == false).Where(bi => bi.BookId == deletedBook.Id).FirstOrDefaultAsync();
                
                
                    string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", hoverImage.ImageUrl);
                    if (File.Exists(path2))
                    {
                        File.Delete(path2);
                    }
                    _context.BookImages.Remove(hoverImage);
                
            }
            if(deletedBook.ImageFiles is not null)
            {
                    List<BookImage> bookImage1 = await _context.BookImages.Where(bi => bi.IsCover == null).Where(bi => bi.BookId == deletedBook.Id).ToListAsync();
                    foreach (var bookImage in bookImage1)
                    {
                        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", bookImage.ImageUrl);
                        if (File.Exists(path2))
                        {
                            File.Delete(path2);
                        }
                        _context.BookImages.Remove(bookImage);
                    } 
            }
            _context.Books.Remove(deletedBook);
            _context.SaveChanges();

        }

        public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? expression = null, params string[] includes)
        {
            var query=_context.Books.AsQueryable();
            query=_GetInclude(query, includes);
            if(expression != null)
            {
                return await query.Where(expression).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        

        public async Task<Book> GetByIdAsync(int id)
        {
            Book book= await _context.Books.FindAsync(id);
            if(book == null) throw new BookNotfoundException("This book is not found!");
            return book;
        }

        public async Task<Book> GetSingleAsync(Expression<Func<Book, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Books.AsQueryable();
            query = _GetInclude(query, includes);
            if (expression is not null)
            {
                return await query.Where(expression).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task UpdateAsync(Book book)
        {
            Book CurrentBook= await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            if (CurrentBook == null) throw new NotFoundException("This book is not found!");
            if ((await _context.Books.FirstOrDefaultAsync(b => b.Name == book.Name) is not null) && (book.Name != CurrentBook.Name))
            {
                throw new NameAlreadyExistException("name", "This Name is already exist!");
            }
            if (book.CoverImageFile is not null)
            {
                if (book.CoverImageFile.ContentType != "image/jpeg" && book.CoverImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
                }
                if (book.CoverImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = book.CoverImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

             
                string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.CoverImageFile.CopyTo(fileStream);
                }

                BookImage? coverImage = await _context.BookImages.Where(bi => bi.IsCover == true).Where(bi => bi.BookId == CurrentBook.Id).FirstOrDefaultAsync();
                
                if (coverImage is not null)
                {
                  string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", coverImage.ImageUrl);
                   if (File.Exists(path2))
                   {
                      File.Delete(path2);
                   }
                   _context.BookImages.Remove(coverImage);
                    
                }
                BookImage bookImage = new BookImage()
                {
                    Book=book,
                    ImageUrl=fileName,
                    IsCover=true,
                    IsActivated=true,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                };          
                await _context.BookImages.AddAsync(bookImage);        
            }
            if (book.HoverImageFile is not null)
            {
                if (book.HoverImageFile.ContentType != "image/jpeg" && book.HoverImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
                }
                if (book.HoverImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = book.HoverImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.HoverImageFile.CopyTo(fileStream);
                }

                 BookImage? hoverImage = await _context.BookImages.Where(bi => bi.IsCover == false).Where(bi => bi.BookId == CurrentBook.Id).FirstOrDefaultAsync();
                if (hoverImage is not null)
                {
                        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", hoverImage.ImageUrl);
                        if (File.Exists(path2))
                        {
                            File.Delete(path2);
                        }
                        _context.BookImages.Remove(hoverImage);                   
                }

                BookImage bookImage = new BookImage()
                {
                    Book=book,
                    ImageUrl = fileName,
                    IsCover = false,
                    IsActivated = true,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                };
                
                await _context.BookImages.AddAsync(bookImage);               
            }

            foreach (var ImageFile in CurrentBook.BookImages.Where(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsCover == null))
            {
                string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", ImageFile.ImageUrl);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }                
            }
            
            CurrentBook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsCover==null);

            if (book.ImageFiles is not null)
            {
                foreach (var ImageFile in book.ImageFiles)
                {
                    if (ImageFile.ContentType != "image/jpeg" && ImageFile.ContentType != "image/png")
                    {
                        throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                    }
                    if (ImageFile.Length > 2097152)
                    {
                        throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                    }
                    string fileName = ImageFile.FileName;
                    if (fileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }
                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }           
                    
                    BookImage bookImage = new BookImage()
                    {
                        Book = book,
                        ImageUrl = fileName,
                        IsCover = null,
                        IsActivated = true,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                    };
                    
                     _context.BookImages.Add(bookImage);
                }

            }
            CurrentBook.Name = book.Name;
            CurrentBook.UpdatedDate = DateTime.UtcNow.AddHours(4);
            CurrentBook.Desc = book.Desc;
            CurrentBook.CostPrice = book.CostPrice;
            CurrentBook.SellPrice = book.SellPrice;
            CurrentBook.Discount = book.Discount;
            CurrentBook.AuthorId = book.AuthorId;
            CurrentBook.IsFeatured = book.IsFeatured;
            CurrentBook.MostView = book.MostView;
            CurrentBook.BookImages = book.BookImages;
            CurrentBook.GenreId = book.GenreId;
            CurrentBook.IsNew = book.IsNew;
            CurrentBook.IsActivated = book.IsActivated;
            CurrentBook.StockCount = book.StockCount;
            CurrentBook.ProductCode = book.ProductCode;
            
            await _context.SaveChangesAsync();
        }
        private IQueryable<Book> _GetInclude(IQueryable<Book> query, params string[] includes)
        {
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
