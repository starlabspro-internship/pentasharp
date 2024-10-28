using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboardd()
        {
            return View();
        }
        public IActionResult CreateCompany()
        {
            return View();
        }
    }
}
