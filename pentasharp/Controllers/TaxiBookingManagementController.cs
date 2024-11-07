using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Controllers
{
    public class TaxiBookingManagementController : Controller
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
        public IActionResult TaxiBookingsManagement()
        {
            return View();
        }

        public IActionResult ManageCurrentBookings()
        {
            return View();
        }
    }
}
