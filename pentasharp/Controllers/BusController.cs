using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BusController : Controller
    {

        public IActionResult BusSchedules()
        {
            return View();
        }

    }
}