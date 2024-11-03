using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusReservationController : Controller
    {

        public IActionResult BusReservations()
        {
            return View();
        }

        public IActionResult BusReservationsManagement()
        {
            return View();
        }
        public IActionResult ConfirmPassengers()
        {
            return View();
        }

        public IActionResult ViewPassangers()
        { 
            return View();
        }
        public IActionResult ManageBusSchedules()
        {
            return View();
        }
    }
}
