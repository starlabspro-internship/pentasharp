using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using pentasharp.Data;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;

namespace pentasharp.Services
{
    public class AdminSetupService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminSetupService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public void EnsureAdminUserExists()
        {
            if (!_context.Users.Any(u => u.IsAdmin))
            {
                var adminDto = new AdminUserDto
                {
                    FirstName = "Penta",
                    LastName = "Sharp",
                    Email = "pentasharp@gmail.com",
                    Password = "Test123!"
                };

                var adminUser = _mapper.Map<User>(adminDto);

                adminUser.PasswordHash = HashPassword(adminDto.Password);

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