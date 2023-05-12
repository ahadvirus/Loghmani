namespace Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;

public class RequestVM
{
    public RequestVM()
    {
        ReturnUrl = string.Empty;
    }

    public string ReturnUrl { get; set; }
}