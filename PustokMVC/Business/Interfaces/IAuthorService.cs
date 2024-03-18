using PustokMVC.Models;
using System.Linq.Expressions;

namespace PustokMVC.Business.Interfaces
{
    public interface IAuthorService
    {
        public Task <List<Author>> GetAllAsync (Expression<Func<Author,bool>>?expression=null,params string[] includes);
        public Task<Author> GetSingleAsync(Expression<Func<Author,bool>>? expression=null);
        public Task <Author> GetByIdAsync(int id);
        public Task Create(Author author);
        public Task Update(Author author);  
        public Task Delete(int id); 
    }
}
