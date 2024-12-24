using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("PrivacyPolicy")]
    public class PrivacyPolicyController : Controller
    {

        [HttpGet("", Name = "PrivacyPolicyIndex")]
        public IActionResult Index()
        {
            return View();
        }
    }
}