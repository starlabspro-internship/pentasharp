using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiReservationController : Controller
    {
        public IActionResult TaxiReservations()
        {
            return View();
        }
        public IActionResult TaxiReservationsManagement()
        {
            return View();
        }
        public IActionResult IncomingReservations()
        {
            return View();
        }
        public IActionResult ManageCurrentReservations()
        {
            return View();
        }
    }
}
