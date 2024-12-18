using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.ViewModel.Taxi;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using pentasharp.Interfaces;

namespace pentasharp.Services
{
    public class TaxiService : ITaxiService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authService;

        public TaxiService(AppDbContext context, IMapper mapper,IAuthenticateService service)
        {
            _context = context;
            _mapper = mapper;
            _authService = service;
        }

        public async Task<List<TaxiRequest>> GetTaxisAsync()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var taxis = _context.Taxis
                .Include(t => t.TaxiCompany)
                .Include(t => t.Driver)
                .Where(t => t.TaxiCompanyId == companyId.Value && !t.IsDeleted)
                .ToList();

            var viewModel = taxis.Select(t => new TaxiRequest
            {
                TaxiId = t.TaxiId,
                LicensePlate = t.LicensePlate,
                TaxiCompanyId = t.TaxiCompanyId,
                CompanyName = t.TaxiCompany.CompanyName,
                DriverId = t.DriverId,
                DriverName = t.DriverId.HasValue
                ? _context.Users
                    .Where(u => u.UserId == t.DriverId.Value)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault()
                : "No Driver Assigned"

            }).ToList();

            return (viewModel);
        }

        public async Task<Taxi> AddTaxiAsync(AddTaxiRequest model)
        {
            if (model.DriverId == null)
            {
                Console.WriteLine("No driver assigned to the taxi.");
            }

            var taxi = _mapper.Map<Taxi>(model);
            _context.Taxis.Add(taxi);
            await _context.SaveChangesAsync();
            return taxi;
        }

        public async Task<bool> EditTaxiAsync(int id, EditTaxiRequest model)
        {
            var taxi = await _context.Taxis.FindAsync(id);
            if (taxi == null)
                return false;

            taxi.LicensePlate = model.LicensePlate;
            taxi.DriverId = model.DriverId;
            taxi.UpdatedAt = DateTime.UtcNow;

            _context.Taxis.Update(taxi);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaxiAsync(int id)
        {
            var taxi = await _context.Taxis.FindAsync(id);
            if (taxi == null)
                return false;

            _context.Taxis.Remove(taxi);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DriverRequest>> GetAvailableDriversAsync(int? taxiId = null)
        {

            var companyId = _authService.GetCurrentCompanyId();
            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var assignedDriverIds = await _context.Taxis
                .Where(t => t.TaxiCompanyId == companyId.Value && !t.IsDeleted && (taxiId == null || t.TaxiId != taxiId))
                .Select(t => t.DriverId)
                .ToListAsync();

            var drivers = await (from u in _context.Users
                                 join c in _context.TaxiCompanies on u.CompanyId equals c.TaxiCompanyId
                                 where u.CompanyId == companyId
                                       && u.Role == UserRole.Driver
                                       && !u.IsDeleted
                                       && !assignedDriverIds.Contains(u.UserId)
                                 select new DriverRequest
                                 {
                                     UserId = u.UserId,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.Email,
                                     CompanyName = c.CompanyName
                                 }).ToListAsync();

            return drivers;
        }

        public TaxiCompany GetCompanyDetails()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var companyDetails = _context.TaxiCompanies
                .Include(tc => tc.User)
                .FirstOrDefault(tc => tc.TaxiCompanyId == companyId.Value);

            return companyDetails;
        }
    }
}