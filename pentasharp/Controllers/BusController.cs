using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusController : Controller
    {

        public IActionResult BusSchedule()
        {
            return View();
        }

        public IActionResult BusReservations()
        {
            return View();
        }
        
        public IActionResult BusReservationsManagement()
        {
            return View();
        }

        public IActionResult BusScheduleManagement()
        {
            return View();
        }

        public IActionResult CreateBusCompany()
        {
            return View();
        }
    }
}
