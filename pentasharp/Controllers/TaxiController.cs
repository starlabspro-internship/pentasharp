using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    public class TaxiController : Controller
    {
        public IActionResult TaxiBooking()
        {
            return View();
        }
        public IActionResult TaxiReservation()
        {
            return View();
        }
    }
} 