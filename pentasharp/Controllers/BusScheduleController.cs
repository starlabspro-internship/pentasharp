using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BusScheduleController : Controller
    {

        public IActionResult BusSchedule()
        {
            return View();
        }

        public IActionResult BusScheduleManagement()
        {
            return View();
        }

        public IActionResult AddBusCompany()
        {
            return View(); 
        }

        public IActionResult ConfirmPassengers()
        {
            return View();
        }

        public IActionResult ManageBusSchedules()
        {
            return View();
        }
    }
}
