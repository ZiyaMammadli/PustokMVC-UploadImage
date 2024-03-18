using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.DAL;
using PustokMVC.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly PustokDbContext _context;
        public AuthorService(PustokDbContext context)
        {
            _context = context;
        }
        public async Task Create(Author author)
        {
            var _author=await _context.Authors.FirstOrDefaultAsync(a=>a.Name.ToLower() == author.Name.ToLower());
            if (_author is not null) throw new NameAlreadyExistException("Name","This name already exist!!");
            author.IsActivated = true;
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var _author= await _context.Authors.FindAsync(id);
            if (_author is null) throw new NotFoundException( "This name is not found!!");
            _context.Authors.Remove(_author);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAllAsync(Expression<Func<Author, bool>>? expression = null, params string[] includes)
        {
            var query=_context.Authors.AsQueryable();
            query=GetInclude(query, includes);
            if(expression != null)
            {
                return await query.Where(expression).ToListAsync();
            }
            else
            {
               return await query.ToListAsync();
            }
                        
        }

        public async Task<Author> GetByIdAsync(int id)
        {
          return await _context.Authors.FindAsync(id);
        }

        public async Task<Author> GetSingleAsync(Expression<Func<Author, bool>>? expression = null)
        {
            var query = _context.Authors.AsQueryable();
            if(expression != null)
            {
                return await query.Where(expression).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task Update(Author author)
        {
            var currentAuthor= await _context.Authors.FirstOrDefaultAsync(a=>a.Id==author.Id);
            if(currentAuthor is null) throw new NotFoundException("Name","This name is not found!!");
            if(await _context.Authors.AnyAsync(a=>a.Name.ToLower()==author.Name.ToLower()) && (currentAuthor.Name.ToLower() != author.Name.ToLower()))
            {
                throw new NameAlreadyExistException("Name", "This name is already exist!!");
            }
            currentAuthor.Name = author.Name;
            _context.SaveChanges();
        }
        private IQueryable<Author> GetInclude(IQueryable<Author> query,params string[] includes) 
        {
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query=query.Include(include);
                }
            }
            return query;
        }
    }
}
