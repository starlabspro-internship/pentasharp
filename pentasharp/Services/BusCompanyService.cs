using AutoMapper;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Bus;
using Microsoft.EntityFrameworkCore;
using pentasharp.Models.DTOs;
using pentasharp.Models.Utilities;

namespace pentasharp.Services
{
    public class BusCompanyService : IBusCompanyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authService;
        private readonly ILogger<BusCompanyService> _logger;

        public BusCompanyService(AppDbContext context, IMapper mapper, IAuthenticateService authService, ILogger<BusCompanyService> logger)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
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

        public async Task<StandardApiResponse<object>> GetBusCompanyUserAsync(int companyId)
        {
            try
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

                if (companyUser?.User == null)
                {

                    return new StandardApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found for the specified Bus Company.",
                        Data = null
                    };
                }

                return new StandardApiResponse<object>
                {
                    Success = true,
                    Message = "User retrieved successfully.",
                    Data = companyUser.User
                };
            }
            catch (Exception ex)
            {
                return new StandardApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user.",
                    Data = null
                };
            }
        }

        public async Task<List<object>> GetBusCompanyUsersAsync()
        {
            var users = await _context.Users
                .Where(user => user.BusinessType == BusinessType.BusCompany)
                .Where(user => user.CompanyId == null)
                .Select(user => new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName
                })
                .ToListAsync<object>();

            if (users == null || !users.Any())
            {
                _logger.LogInformation("No bus company users were found.");
                return null;
            }

            return users;
        }



        public async Task<object> GetCompanyByUserIdAsync()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var company = await _context.BusCompanies
                .FirstOrDefaultAsync(c => c.BusCompanyId == companyId.Value);

            if (company == null) return null;

            return new
            {
                company.BusCompanyId,
                company.CompanyName
            };
        }

        public List<BusCompanyViewModel> GetAllBusCompanies()
        {
            var companies = _context.BusCompanies.ToList();

            return _mapper.Map<List<BusCompanyViewModel>>(companies ?? new List<BusCompany>());
        }
    }
}