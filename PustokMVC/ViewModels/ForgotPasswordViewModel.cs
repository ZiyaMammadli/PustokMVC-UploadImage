using System.ComponentModel.DataAnnotations;

namespace PustokMVC.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }   
    }
}
