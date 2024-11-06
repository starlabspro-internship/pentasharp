using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult AllNotifications()
        {
            return View();
        }
    }
}
