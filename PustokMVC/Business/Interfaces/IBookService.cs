using PustokMVC.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Interfaces
{
    public interface IBookService
    {
        public Task<List<Book>> GetAllAsync(Expression<Func<Book,bool>>?expression=null,params string[]includes);
        public Task <Book> GetByIdAsync(int id);
        public Task<Book> GetSingleAsync(Expression<Func<Book, bool>>? expression = null);
        public Task CreateAsync(Book book);
        public Task UpdateAsync(Book book);
        public Task DeleteAsync(int id);
    }
}
