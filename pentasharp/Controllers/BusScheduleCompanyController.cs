using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class BusScheduleCompanyController : Controller
	{
		public IActionResult AddBusCompany()
		{
			return View();
		}
	}
}
