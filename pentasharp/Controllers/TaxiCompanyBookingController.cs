using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AdminOnlyFilter]
    public class TaxiCompanyBookingController : Controller
    {
        public IActionResult AddTaxiCompany()
        {
            return View();
        }
    }
}