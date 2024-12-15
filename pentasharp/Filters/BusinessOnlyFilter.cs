using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApplication1.Filters
{
    public class BusinessOnlyFilter : ActionFilterAttribute
    {
        private readonly string _requiredBusinessType;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusinessOnlyFilter(string requiredBusinessType, IHttpContextAccessor httpContextAccessor)
        {
            _requiredBusinessType = requiredBusinessType ?? throw new ArgumentNullException(nameof(requiredBusinessType));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var businessType = _httpContextAccessor.HttpContext?.Session.GetString("BusinessType");
            var role = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");

            if (businessType != _requiredBusinessType && role != "Admin")
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}