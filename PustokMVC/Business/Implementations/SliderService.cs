using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PustokMVC.Business.Interfaces;
using PustokMVC.CustomExceptions.Common;
using PustokMVC.DAL;
using PustokMVC.Models;
using System;
using System.Linq.Expressions;

namespace PustokMVC.Business.Implementations;

public class SliderService : ISliderService
{
    private readonly IWebHostEnvironment _env;
    private readonly PustokDbContext _context;
    public SliderService(PustokDbContext context,IWebHostEnvironment env)
    {
        _context = context;
        _env=env;
    }
    public async Task CreateAsync(Slider slider)
    {
        if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");   
        }
        if (slider.ImageFile.Length > 2097152)
        {
            throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
        }
        string fileName = slider.ImageFile.FileName;
        if (fileName.Length > 64)
        {
            fileName = fileName.Substring(fileName.Length - 64, 64);
        }
        fileName = Guid.NewGuid().ToString() + fileName;

        string path = Path.Combine(_env.WebRootPath,"uploads/sliders",fileName);

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            slider.ImageFile.CopyTo(fileStream);
        }

        slider.CreatedDate = DateTime.UtcNow.AddHours(4);
        slider.UpdatedDate = DateTime.UtcNow.AddHours(4);
        slider.ImageUrl = fileName;
        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();       
    }

    public async Task DeleteAsync(int id)
    {
        Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider == null) throw new NotFoundException("This Slider is not found!");
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
    {
        var query=_context.Sliders.AsQueryable();
        _GetIncludes(query, includes);
        if(expression is not null)
        {
            return await query.Where(expression).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public async Task<Slider> GetByIdAsync(int id)
    {
        return await _context.Sliders.FindAsync(id);
    }

    public async Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null)
    {
        var query = _context.Sliders.AsQueryable();
        if(expression is not null)
        {
            return await query.Where(expression).FirstOrDefaultAsync();
        }
        else
        {
            return await query.FirstOrDefaultAsync();
        }
    }

    public async Task UpdateAsync(Slider slider)
    {
        Slider currentSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == slider.Id);
        if (currentSlider == null) throw new NotFoundException("This Slider is not found!");
        if (slider.ImageFile.Length > 2097152)
        {
            throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
        }
        if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
        }

        string fileName = slider.ImageFile.FileName;
        if (fileName.Length > 64)
        {
            fileName = fileName.Substring(fileName.Length - 64, 64);
        }
        fileName = Guid.NewGuid().ToString() + fileName;

        string path = Path.Combine(_env.WebRootPath, "uploads/sliders", fileName);
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            slider.ImageFile.CopyTo(fileStream);

        }
        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", currentSlider.ImageUrl);

        if (File.Exists(path2))
        {
            File.Delete(path2);
        }


        slider.UpdatedDate = DateTime.UtcNow.AddHours(4);
        currentSlider.ImageUrl = fileName;
        currentSlider.Title1 = slider.Title1;
        currentSlider.Title2 = slider.Title2;
        currentSlider.Desc = slider.Desc;
        currentSlider.RedirectText = slider.RedirectText;
        currentSlider.RedirectUrl = slider.RedirectUrl;
        await _context.SaveChangesAsync();
    }
    private IQueryable <Slider>_GetIncludes(IQueryable <Slider> query,params string[] includes)
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
