using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [LoginRequiredFilter]
    public class NotificationController : Controller
    {
        public IActionResult AllNotifications()
        {
            return View();
        }
    }
}