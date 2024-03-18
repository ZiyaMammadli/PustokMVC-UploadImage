using Microsoft.EntityFrameworkCore;
using PustokMVC.Models;

namespace PustokMVC.DAL
{
    public class PustokDbContext: DbContext
    {
        public PustokDbContext(DbContextOptions <PustokDbContext>option):base(option) { }

        public DbSet<Slider> Sliders {  get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Author> Authors { get; set; }   

    }
}
