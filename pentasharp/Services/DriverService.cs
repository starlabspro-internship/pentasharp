using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.Authenticate;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DriverService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> AddDriverAsync(RegisterDriverRequest model, int companyId)
        {
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                CompanyId = companyId,
                Role = UserRole.Driver,
                IsAdmin = false,
                BusinessType = BusinessType.TaxiCompany
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public List<DriverRequest> GetDrivers(int companyId)
        {
            var drivers = (from u in _context.Users
                           join c in _context.TaxiCompanies on u.CompanyId equals c.TaxiCompanyId
                           where u.CompanyId == companyId
                                 && u.Role == UserRole.Driver
                                 && u.BusinessType == BusinessType.TaxiCompany
                                 && !u.IsDeleted
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
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.IsDeleted || user.Role != UserRole.Driver)
                return false;

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
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.IsDeleted || user.Role != UserRole.Driver)
                return false;

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