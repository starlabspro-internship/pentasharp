using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusScheduleController : Controller
    {
        public IActionResult BusScheduleManagement()
        {
            return View();
        }

        public IActionResult ManageBusSchedules()
        {
            return View();
        }
    }
}