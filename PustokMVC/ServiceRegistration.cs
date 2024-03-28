using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Implementations;
using PustokMVC.Business.Interfaces;
using PustokMVC.DAL;
using PustokMVC.Models;

namespace PustokMVC;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<ISliderService, SliderService>();
        services.AddScoped<IBookService, BookService>();
        services.AddIdentity<AppUser, IdentityRole>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireDigit = true;
            opt.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<PustokDbContext>()
            .AddDefaultTokenProviders();
        services.AddDbContext<PustokDbContext>(opt =>
        {
            opt.UseSqlServer("Server=WIN-PRIFU0D7GO7\\SQLEXPRESS;Database=PustokDb;Trusted_Connection=true;TrustServerCertificate=True");
        });

    }
}
