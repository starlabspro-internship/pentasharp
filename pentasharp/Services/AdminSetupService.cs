using System.Security.Cryptography;
using System.Text;
using pentasharp.Data;
using pentasharp.Models.Entities;

namespace pentasharp.Services
{
    public class AdminSetupService
    {
        private readonly AppDbContext _context;

        public AdminSetupService(AppDbContext context)
        {
            _context = context;
        }

        public void EnsureAdminUserExists()
        {
            if (!_context.Users.Any(u => u.IsAdmin))
            {
                var adminUser = new User
                {
                    FirstName = "Penta",
                    LastName = "Sharp",
                    Email = "pentasharp@gmail.com",
                    PasswordHash = HashPassword("Test123!"),
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(adminUser);
                _context.SaveChanges();
            }
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
    }
}
