using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiReservationManagementController : Controller
    {
        public IActionResult TaxiReservationsManagement()
        {
            return View();
        }

        public IActionResult ManageCurrentReservations()
        {
            return View();
        }
    }
}
