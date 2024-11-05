using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class IncomingTaxiReservationController : Controller
    {
        public IActionResult IncomingReservations()
        {
            return View();
        }
    }
}
