using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Models
{
    public class Author:BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } 
        public List <Book>? Books { get; set; }
    }
}
