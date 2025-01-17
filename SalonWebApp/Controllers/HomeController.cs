using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "ADMIN")]
    public IActionResult IndexAdmin()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
