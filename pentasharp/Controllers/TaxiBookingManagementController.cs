using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiBookingManagementController : Controller
    {
        public IActionResult TaxiBookingsManagement()
        {
            return View();
        }

        public IActionResult ManageCurrentBookings()
        {
            return View();
        }
    }
}
