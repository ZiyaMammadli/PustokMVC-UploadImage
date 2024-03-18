using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Models
{
    public class BookImage:BaseEntity
    {
        public int BookId { get; set; }
        [StringLength(100)]
        public string? ImageUrl { get; set; }
        public bool? IsCover { get; set; } //true=cover sekli , false=back sekli , null=detail sekilleri
        public Book? Book { get; set; }
    }
}
