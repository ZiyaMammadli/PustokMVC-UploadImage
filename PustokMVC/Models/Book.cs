
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokMVC.Models
{
    public class Book:BaseEntity
    {
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Desc { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public double Discount {  get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool MostView { get; set; }
        public int StockCount { get; set; }
        public int ProductCode { get; set; }
        [NotMapped]
        public IFormFile? CoverImageFile { get; set; }
        [NotMapped]
        public IFormFile? HoverImageFile { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public List <int>? BookImageIds { get; set; }
        public Author? Author { get; set; }
        public Genre? Genre { get; set; }
        public List <BookImage>? BookImages { get; set; }

    }
}
