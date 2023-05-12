using Loghmani.ECommerce.Old.Infrastructures.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Loghmani.ECommerce.Old.Areas.Admin.Controllers;

[Area(nameof(Area.Admin))]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}