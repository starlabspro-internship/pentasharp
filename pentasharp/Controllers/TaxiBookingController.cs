using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AdminOnlyFilter]
    public class TaxiBookingController : Controller
	{
        public IActionResult IncomingBookings()
		{
			return View();
		}
	}
}