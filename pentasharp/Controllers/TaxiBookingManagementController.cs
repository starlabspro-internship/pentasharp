using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
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