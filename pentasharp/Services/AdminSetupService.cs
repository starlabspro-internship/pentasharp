using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using pentasharp.Data;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;

namespace pentasharp.Services
{
    public class AdminSetupService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly AdminUserDto _adminUserDto;

        public AdminSetupService(AppDbContext context, IMapper mapper, IOptions<AdminUserDto> adminUserDto)
        {
            _context = context;
            _mapper = mapper;
            _adminUserDto = adminUserDto.Value;
        }

        public void EnsureAdminUserExists()
        {
            if (!_context.Users.Any(u => u.IsAdmin))
            {

                var adminUser = _mapper.Map<User>(_adminUserDto);

                adminUser.PasswordHash = HashPassword(_adminUserDto.Password);

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