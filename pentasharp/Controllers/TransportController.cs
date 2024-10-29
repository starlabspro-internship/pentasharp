using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TransportController : Controller
    {
        public IActionResult BusSchedule()
        {
            return View();
        }

        public IActionResult Taxis()
        {
            return View();
        }
    }
}