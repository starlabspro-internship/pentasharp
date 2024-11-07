using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filters
{
    public class LoginRequiredFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("UserId") != null;

            if (!isLoggedIn)
            {
                context.Result = new RedirectToActionResult("Login", "Authenticate", null);
            }
        }
    }
}