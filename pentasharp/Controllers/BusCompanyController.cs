using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusCompanyController : Controller
    {
        
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddBus()
        {
            return View();
        }
    }
}