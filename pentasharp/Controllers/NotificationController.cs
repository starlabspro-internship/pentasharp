using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Controllers
{
    public class NotificationController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = HttpContext.Session.GetString("UserId") != null;

            if (!isLoggedIn)
            {
                context.Result = RedirectToAction("Login", "Authenticate");
            }

            base.OnActionExecuting(context);
        }
        public IActionResult AllNotifications()
        {
            return View();
        }
    }
}
