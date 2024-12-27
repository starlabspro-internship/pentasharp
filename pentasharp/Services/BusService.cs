using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.Bus;

namespace pentasharp.Services
{
    public class BusService : IBusService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _authService;

        public BusService(AppDbContext context, IMapper mapper, IAuthenticateService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<bool> AddBusAsync(AddBusViewModel model)
        {
            var userId = _authService.GetCurrentUserId();

            if (!userId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId.Value && u.BusinessType == BusinessType.BusCompany);
            if (user == null) return false;

            var bus = _mapper.Map<Buses>(model);
            await _context.Buses.AddAsync(bus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BusViewModel>> GetBusesAsync()
        {

            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var buses = await _context.Buses
                .Where(b => b.BusCompanyId == companyId.Value)
                .Include(b => b.BusCompany)
                .ToListAsync();

            return buses.Select(b => new BusViewModel
            {
                BusId = b.BusId,
                BusNumber = b.BusNumber,
                Capacity = b.Capacity,
                BusCompanyId = b.BusCompanyId,
                CompanyName = b.BusCompany.CompanyName
            }).ToList();
        }

        public async Task<bool> EditBusAsync(int id, EditBusViewModel model)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var bus = await _context.Buses.Include(b => b.BusCompany).FirstOrDefaultAsync(b => b.BusId == id);
            if (bus == null || bus.BusCompany.BusCompanyId != companyId.Value) return false;

            bus.BusNumber = model.BusNumber;
            bus.Capacity = model.Capacity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusAsync(int id)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var bus = await _context.Buses.Include(b => b.BusCompany).FirstOrDefaultAsync(b => b.BusId == id);
            if (bus == null || bus.BusCompany.BusCompanyId != companyId.Value) return false;

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}