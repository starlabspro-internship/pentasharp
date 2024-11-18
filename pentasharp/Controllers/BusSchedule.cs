using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
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
        public IActionResult BusReservationManagement()
        {
            return View();
        }
    }
}