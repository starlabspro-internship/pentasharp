using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AuthenticateController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
