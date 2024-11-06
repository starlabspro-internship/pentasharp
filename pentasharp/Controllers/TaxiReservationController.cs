using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiReservationController : Controller
    {
        public IActionResult TaxiReservations()
        {
            return View();
        }
    }
}
