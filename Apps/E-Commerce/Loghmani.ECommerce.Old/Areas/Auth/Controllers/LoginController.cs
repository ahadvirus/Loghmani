using Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;
using Loghmani.ECommerce.Old.Infrastructures.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Loghmani.ECommerce.Old.Areas.Auth.Controllers;

[Area(nameof(Area.Auth))]
public class LoginController : Controller
{
    public IActionResult Index([Bind(nameof(RequestVM.ReturnUrl))] RequestVM request)
    {
        return View();
    }
}