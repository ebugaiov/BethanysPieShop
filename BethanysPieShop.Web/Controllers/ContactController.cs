using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Web.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}