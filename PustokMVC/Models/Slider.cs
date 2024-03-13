using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokMVC.Models;

public class Slider:BaseEntity
{
    [Required]
    [StringLength(40)]
    public string Title1 { get; set; }
    [Required]
    [StringLength(40)]
    public string Title2 { get; set; }
    [Required]
    [StringLength(250)] 
    public string Desc {  get; set; }
    public string RedirectUrl { get; set; }
    [Required]
    [StringLength(40)]
    public string RedirectText { get; set; }
    [StringLength(100)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
}
