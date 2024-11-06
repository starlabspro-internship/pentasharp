using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TaxiCompanyBookingController : Controller
    {
        public IActionResult AddTaxiCompany()
        {
            return View();
        }
    }
}
