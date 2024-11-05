using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusSchedulePassengerController : Controller
    {
        public IActionResult ConfirmPassengers()
        {
            return View();
        }
    }
}
