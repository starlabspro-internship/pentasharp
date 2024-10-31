using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class TaxiController : Controller
	{
		public IActionResult TaxiReservations()
		{
			return View();
		}

		public IActionResult TaxiBookings()
		{
			return View();
		}

        public IActionResult TaxiBookingsManagement()
        {
            return View();
        }

        public IActionResult TaxiReservationsManagement()
        {
            return View();
        }
		 
        public IActionResult CreateTaxiCompany()
		{
			return View();
		}
	}
}
