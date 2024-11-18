using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiManagementController : Controller
    {

        public IActionResult TaxiBookingManagement()
        {
            return View();
        }

        public IActionResult TaxiReservationManagement()
        {
            return View();
        }

    }
}