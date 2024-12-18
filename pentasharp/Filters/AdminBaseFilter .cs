using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApplication1.Filters
{
    public class AdminBaseFilter : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminBaseFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            var isAdmin = session?.GetString("IsAdmin") == "true";
            var role = session?.GetString("UserRole");

            if ((isAdmin || role == "Admin") == false)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}