using System.ComponentModel.DataAnnotations;
using Loghmani.ECommerce.Old.Infrastructures.Configurations;

namespace Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;

public class LoginVM : RequestVM
{
    public LoginVM()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    [Display(Name = nameof(Username))]
    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(Username) + nameof(DataAnnotations.Required) + nameof(RequiredAttribute.ErrorMessage))]
    public string Username { get; set; }

    [Display(Name = nameof(Password))]
    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(Password) + nameof(DataAnnotations.Required) + nameof(RequiredAttribute.ErrorMessage))]
    public string Password { get; set; }
}