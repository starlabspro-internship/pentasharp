using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class TaxiDriverController : Controller
    {
        public IActionResult TaxiDriver()
        {
            return View();
        }
    }
}