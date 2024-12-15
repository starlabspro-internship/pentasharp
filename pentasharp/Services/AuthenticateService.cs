using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Authenticate;
using pentasharp.Models.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace pentasharp.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthenticateService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        } 

        public async Task<User> RegisterAsync(RegisterViewModel model)
        {
            var user = _mapper.Map<User>(model);
            user.PasswordHash = HashPassword(model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.AsNoTracking()
                                .SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            if (user.PasswordHash != HashPassword(password))
                return null;

            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> EditUserAsync(int userId, EditUserViewModel model)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _mapper.Map(model, user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = HashPassword(model.Password);
            }

            if (model.Role.HasValue)
            {
                user.Role = model.Role.Value;
            }

            if (model.BusinessType.HasValue)
            {
                user.BusinessType = model.BusinessType.Value;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !user.IsDeleted)
                return false;

            user.IsDeleted = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetCurrentUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
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