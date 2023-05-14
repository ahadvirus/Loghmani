using Microsoft.AspNetCore.Mvc;

namespace Loghmani.ECommerce.Old.Areas.Auth.Controllers;

public class AccessController : Controller
{
    // GET
    public IActionResult Denied()
    {
        return Ok();
    }
}