using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{

    [AllowAnonymous]
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