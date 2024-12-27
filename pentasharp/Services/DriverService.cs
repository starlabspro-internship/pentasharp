using pentasharp.Interfaces;
using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using pentasharp.Data;
using pentasharp.Models.Enums;

namespace pentasharp.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authService;

        public DriverService(AppDbContext context, IMapper mapper, IAuthenticateService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<User> AddDriverAsync(RegisterDriverRequest model)
        {
            var companyId = _authService.GetCurrentCompanyId();
            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var company = await _context.TaxiCompanies.FirstOrDefaultAsync(c => c.TaxiCompanyId == companyId.Value);
            if (company == null)
            {
                throw new InvalidOperationException("No associated taxi company found for this user.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && !u.IsDeleted);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var newDriver = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                CompanyId = companyId.Value,
                Role = UserRole.Driver,
                IsAdmin = false,
                BusinessType = BusinessType.TaxiCompany,
                IsDeleted = false
            };

            _context.Users.Add(newDriver);
            await _context.SaveChangesAsync();
            return newDriver;
        }

        public List<DriverRequest> GetDrivers()
        {
            var companyId = _authService.GetCurrentCompanyId();
            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var filteredUsers = _context.Users
                .Where(u => u.CompanyId == companyId.Value
                            && u.Role == UserRole.Driver
                            && u.BusinessType == BusinessType.TaxiCompany
                            && !u.IsDeleted);

            var drivers = (from u in filteredUsers
                           join c in _context.TaxiCompanies on u.CompanyId equals c.TaxiCompanyId
                           select new DriverRequest
                           {
                               UserId = u.UserId,
                               FirstName = u.FirstName,
                               LastName = u.LastName,
                               Email = u.Email,
                               CompanyName = c.CompanyName
                           }).ToList();

            return drivers;
        }

        public async Task<bool> EditDriverAsync(int id, EditDriverRequest model)
        {
            var companyId = _authService.GetCurrentCompanyId();
            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id
                                                                    && u.CompanyId == companyId.Value
                                                                    && u.Role == UserRole.Driver
                                                                    && !u.IsDeleted);
            if (user == null)
            {
                return false;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = HashPassword(model.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDriverAsync(int id)
        {
            var companyId = _authService.GetCurrentCompanyId();
            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id
                                                                    && u.CompanyId == companyId.Value
                                                                    && u.Role == UserRole.Driver
                                                                    && !u.IsDeleted);
            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
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