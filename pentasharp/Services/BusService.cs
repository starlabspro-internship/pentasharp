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

        public BusService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddBusAsync(AddBusViewModel model, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.BusinessType == BusinessType.BusCompany);
            if (user == null) return false;

            var bus = _mapper.Map<Buses>(model);
            await _context.Buses.AddAsync(bus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BusViewModel>> GetBusesAsync(int userId)
        {
            var company = await _context.BusCompanies.FirstOrDefaultAsync(c => c.UserId == userId);
            if (company == null) return null;

            var buses = await _context.Buses
                .Where(b => b.BusCompanyId == company.BusCompanyId)
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

        public async Task<bool> EditBusAsync(int id, EditBusViewModel model, int userId)
        {
            var bus = await _context.Buses.Include(b => b.BusCompany).FirstOrDefaultAsync(b => b.BusId == id);
            if (bus == null || bus.BusCompany.UserId != userId) return false;

            bus.BusNumber = model.BusNumber;
            bus.Capacity = model.Capacity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusAsync(int id, int userId)
        {
            var bus = await _context.Buses.Include(b => b.BusCompany).FirstOrDefaultAsync(b => b.BusId == id);
            if (bus == null || bus.BusCompany.UserId != userId) return false;

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}