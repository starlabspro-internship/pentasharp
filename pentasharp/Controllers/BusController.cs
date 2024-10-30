using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusController : Controller
    {

        public IActionResult BusSchedule()
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
    }
}
