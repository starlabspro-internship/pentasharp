using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class TaxiController : Controller
	{
		public IActionResult TaxiReservations()
		{
			return View();
		}

		public IActionResult TaxiManagement()
		{
			return View();
		}

		public IActionResult CreateTaxiCompany()
		{
			return View();
		}
	}
}
