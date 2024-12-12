using AutoMapper;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Bus;
using Microsoft.EntityFrameworkCore;

namespace pentasharp.Services
{
    public class BusCompanyService : IBusCompanyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BusCompanyService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddCompanyAsync(BusCompanyViewModel model)
        {
            if (model == null) return false;

            var company = _mapper.Map<BusCompany>(model);
            await _context.BusCompanies.AddAsync(company);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (user != null)
            {
                user.CompanyId = company.BusCompanyId;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<BusCompanyViewModel>> GetCompaniesAsync()
        {
            var companies = await _context.BusCompanies.Include(c => c.Buses).ToListAsync();
            return _mapper.Map<List<BusCompanyViewModel>>(companies);
        }

        public async Task<bool> EditCompanyAsync(int id, BusCompanyViewModel model)
        {
            var company = await _context.BusCompanies.FindAsync(id);
            if (company == null) return false;

            _mapper.Map(model, company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.BusCompanies.Include(c => c.Buses)
                .FirstOrDefaultAsync(c => c.BusCompanyId == id);
            if (company == null) return false;

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;

            var users = await _context.Users.Where(u => u.CompanyId == id).ToListAsync();
            foreach (var user in users)
            {
                user.CompanyId = null;
                _context.Users.Update(user);
            }

            foreach (var bus in company.Buses)
            {
                bus.IsDeleted = true;
                bus.UpdatedAt = DateTime.UtcNow;
            }

            _context.BusCompanies.Update(company);
            _context.Buses.UpdateRange(company.Buses);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<object> GetBusCompanyUserAsync(int companyId)
        {
            var companyUser = await _context.BusCompanies
                .Where(tc => tc.BusCompanyId == companyId)
                .Select(tc => new
                {
                    User = _context.Users
                        .Where(u => u.CompanyId == companyId)
                        .Where(c => c.BusinessType == BusinessType.BusCompany)
                        .Select(u => new
                        {
                            u.UserId,
                            u.FirstName,
                            u.LastName
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            return companyUser;
        }

        public async Task<List<object>> GetBusCompanyUsersAsync()
        {
            return await _context.Users
                .Where(user => user.BusinessType == BusinessType.BusCompany)
                .Where(user => user.CompanyId == null)
                .Select(user => new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                })
                .ToListAsync<object>();
        }


        public async Task<object> GetCompanyByUserIdAsync(int userId)
        {
            var company = await _context.BusCompanies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null) return null;

            return new
            {
                company.BusCompanyId,
                company.CompanyName
            };
        }
    }
}