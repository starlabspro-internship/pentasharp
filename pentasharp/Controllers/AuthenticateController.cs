using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using pentasharp.Data;
using pentasharp.Models.Entities;
using System.Text;
using pentasharp.ViewModel.Authenticate;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.DTOs;
using pentasharp.Models.Enums;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("Authenticate")]
    public class AuthenticateController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<AuthenticateController> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Use AutoMapper to map the model to the User entity
                var user = _mapper.Map<User>(model);
                user.PasswordHash = HashPassword(model.Password);  // Handle PasswordHash separately

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet("UserList")]
        [ServiceFilter(typeof(AdminOnlyFilter))]
        public IActionResult UserList()
        {
        
            var users = _context.Users.ToList();

            return View(users);
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // Map User to EditUserViewModel
            var model = _mapper.Map<EditUserViewModel>(user);
            return View(model);
        }

        [HttpPost("EditUser")]
        public IActionResult Edit(EditUserViewModel model)
        {
            var user = _context.Users.Find(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Map the updated properties from the view model to the User entity
            _mapper.Map(model, user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = HashPassword(model.Password);  // Update password if provided
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("UserList");
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost("DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
        }
        [HttpPost("Restore/{id}")]
        public IActionResult Restore(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null && user.IsDeleted)
            {
                user.IsDeleted = false;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid input.");
                return View(model);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for user {Email}. Error: {ErrorCode} - {ErrorMessage}", model.Email, ApiStatusEnum.USER_NOT_FOUND, "User not found.");

                ModelState.AddModelError(string.Empty, "User not found.");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new StandardResponse(ApiStatusEnum.USER_NOT_FOUND, model.Email, "User not found."));
                }

                return View(model);
            }

            if (user.PasswordHash != HashPassword(model.Password))
            {
                _logger.LogWarning("Login failed for user {Email}. Error: {ErrorCode} - {ErrorMessage}", model.Email, ApiStatusEnum.LOGIN_FAILED, "Incorrect password.");

                ModelState.AddModelError(string.Empty, "Incorrect password.");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new StandardResponse(ApiStatusEnum.LOGIN_FAILED, model.Email, "Incorrect password."));
                }

                return View(model);
            }

            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("UserId", user.UserId.ToString());
            session.SetString("FirstName", user.FirstName);
            session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

            _logger.LogInformation("User {Email} logged in successfully.", model.Email);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new StandardResponse(ApiStatusEnum.OK, model.Email, "Login successful."));
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("UserId");

            _logger.LogInformation("User logged out successfully.");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new StandardResponse(
                    ApiStatusEnum.UNAUTHORIZED,
                    Guid.NewGuid().ToString(),
                    "User is not authorized to access this resource."
                ));
            }

            var user = _context.Users.Find(int.Parse(userId));
            if (user == null)
            {
                return NotFound(new StandardResponse(
                    ApiStatusEnum.USER_NOT_FOUND,
                    Guid.NewGuid().ToString(),
                    "User not found."
                ));
            }

            return Ok(new StandardResponse(
                ApiStatusEnum.OK,
                Guid.NewGuid().ToString(),
                "Success",
                new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                }
            ));
        }
    }
}