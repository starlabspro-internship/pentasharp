using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult BusManagement()
        {
            return View();
        }
        public IActionResult CreateBusCompany()
        {
            return View();
        }
        public IActionResult TaxiManagement()
        {
            return View();
        }
        public IActionResult CreateTaxiCompany()
        {
            return View();
        }
    }
}
