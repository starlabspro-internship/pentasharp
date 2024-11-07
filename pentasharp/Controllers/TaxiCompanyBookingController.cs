using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Controllers
{
    public class TaxiCompanyBookingController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            if (!isAdmin)
            {
                context.Result = RedirectToAction("Index", "Home");
            }

            base.OnActionExecuting(context);
        }
        public IActionResult AddTaxiCompany()
        {
            return View();
        }
    }
}
