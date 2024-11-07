using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiController : Controller
    {
        public IActionResult TaxiBookings()
        {
            return View();
        }
        public IActionResult TaxiReservations()
        {
            return View();
        }
    }
}