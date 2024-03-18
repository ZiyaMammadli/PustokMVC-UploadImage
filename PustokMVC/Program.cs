using Microsoft.EntityFrameworkCore;
using PustokMVC.Business.Implementations;
using PustokMVC.Business.Interfaces;
using PustokMVC.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddDbContext<PustokDbContext>(opt =>
{
    opt.UseSqlServer("Server=WIN-PRIFU0D7GO7\\SQLEXPRESS;Database=PustokDb;Trusted_Connection=true;TrustServerCertificate=True");
});

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
