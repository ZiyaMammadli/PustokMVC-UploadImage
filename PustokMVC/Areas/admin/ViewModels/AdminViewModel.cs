using System.ComponentModel.DataAnnotations;

namespace PustokMVC.Areas.admin.ViewModels;

public class AdminViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
