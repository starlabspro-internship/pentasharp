using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.ViewModel.Taxi;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;

namespace pentasharp.Services
{
    public class TaxiService : ITaxiService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaxiService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaxiRequest>> GetTaxisAsync(int companyId)
        {
            var taxis = _context.Taxis
                .Include(t => t.TaxiCompany)
                .Include(t => t.Driver)
                .Where(t => t.TaxiCompanyId == companyId && !t.IsDeleted)
                .ToList();

            var viewModel = taxis.Select(t => new TaxiRequest
            {
                TaxiId = t.TaxiId,
                LicensePlate = t.LicensePlate,
                TaxiCompanyId = t.TaxiCompanyId,
                CompanyName = t.TaxiCompany?.CompanyName,
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

        public async Task<List<DriverRequest>> GetAvailableDriversAsync(int companyId, int? taxiId = null)
        {

            var assignedDriverIds = await _context.Taxis
                .Where(t => t.TaxiCompanyId == companyId && !t.IsDeleted && (taxiId == null || t.TaxiId != taxiId))
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
    }
}