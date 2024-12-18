using Microsoft.AspNetCore.Mvc;
using pentasharp.Data;
using pentasharp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace pentasharp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewsController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _context.UserReviews.ToListAsync();
            return View("Index", reviews); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(UserReview review)
        {
            if (ModelState.IsValid)
            {
                string userName = "Anonymous";

                var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId.Value);
                    userName = user?.FirstName ?? userName; 
                    review.UserId = userId.Value;
                }

                review.UserName = string.IsNullOrEmpty(review.UserName) ? userName : review.UserName;

                _context.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/Home/Index.cshtml", review);
        }

        [HttpGet]
        public IActionResult SubmitReview()
        {
            return View(new UserReview()); 
        }
    }
}
