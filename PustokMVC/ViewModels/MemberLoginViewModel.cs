using System.ComponentModel.DataAnnotations;

namespace PustokMVC.ViewModels;

public class MemberLoginViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
