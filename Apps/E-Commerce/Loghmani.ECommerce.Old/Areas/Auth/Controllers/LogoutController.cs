using Loghmani.ECommerce.Old.Infrastructures.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Loghmani.ECommerce.Old.Areas.Auth.Controllers;

public class LogoutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return RedirectToAction(nameof(LoginController.Index), nameof(LoginController).RemoveController());
    }
}