using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "TaxiCompany" })]
    public class TaxiDriverController : Controller
    {
        public IActionResult TaxiDriver()
        {
            return View();
        }
    }
}