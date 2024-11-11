using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApplication1.Filters
{
    public class AdminOnlyFilter : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminOnlyFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = _httpContextAccessor.HttpContext?.Session.GetString("IsAdmin") == "true";

            if (!isAdmin)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}