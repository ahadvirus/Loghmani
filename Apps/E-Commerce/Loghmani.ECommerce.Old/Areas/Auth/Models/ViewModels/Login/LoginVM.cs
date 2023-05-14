namespace Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;

public class LoginVM : RequestVM
{
    public LoginVM()
    {
        Username = string.Empty;
        Password = string.Empty;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}