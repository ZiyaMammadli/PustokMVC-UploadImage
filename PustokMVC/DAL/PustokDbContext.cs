using Microsoft.EntityFrameworkCore;
using PustokMVC.Models;

namespace PustokMVC.DAL
{
    public class PustokDbContext: DbContext
    {
        public PustokDbContext(DbContextOptions <PustokDbContext>option):base(option) { }

        public DbSet<Slider> Sliders {  get; set; }
    }
}
