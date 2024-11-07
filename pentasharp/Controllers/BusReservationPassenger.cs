using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AdminOnlyFilter]
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