using System.ComponentModel.DataAnnotations;

namespace PustokMVC.ViewModels;

public class RegisterViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string FullName { get; set; }
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    [Compare("PasswordConfirmed")]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    public string PasswordConfirmed { get; set; }
    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    public string Email { get; set; }
    [DataType(DataType.PhoneNumber)]
    [StringLength(25)]
    public string PhoneNumber { get; set; }



}
