using Microsoft.AspNetCore.Mvc;

namespace pentasharp.Controllers
{
    public class BusController:Controller
    {
        public IActionResult BusSchedule()
        {
            return View();
        }

        public IActionResult BusReservations()
        {
            return View();
        }
    }
}