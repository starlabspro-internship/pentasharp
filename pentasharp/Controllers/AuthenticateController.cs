using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using pentasharp.Data;
using pentasharp.Models.Entities;
using System.Text;
using pentasharp.ViewModel.Authenticate;

namespace WebApplication1.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly AppDbContext _context;

        public AuthenticateController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password)
                };

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

            var model = new EditUserViewModel
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

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

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = HashPassword(model.Password);
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
}
