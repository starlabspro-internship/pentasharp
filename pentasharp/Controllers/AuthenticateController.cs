using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using pentasharp.Data;
using pentasharp.Models.Entities;
using System.Text;
using pentasharp.ViewModel.Authenticate;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthenticateController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
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
        public IActionResult UserList()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            if (!isAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.ToList();
            return View(users);
        }


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

        [HttpPost]
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
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || user.PasswordHash != HashPassword(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Email or password are incorrect.");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("UserId");
            return RedirectToAction("Index", "Home");
        }
    }
}