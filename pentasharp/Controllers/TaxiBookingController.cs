using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class TaxiBookingController : Controller
	{
		public IActionResult TaxiBookings()
		{
			return View();
		}

		public IActionResult IncomingBookings()
		{
			return View();
		}
	}
}
