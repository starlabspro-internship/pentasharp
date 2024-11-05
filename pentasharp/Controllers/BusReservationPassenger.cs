using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusReservationPassengerController : Controller
    {
        public IActionResult ConfirmPassengers()
        {
            return View();
        }

        public IActionResult ViewPassengers()
        {
            return View();
        }
    }
}
