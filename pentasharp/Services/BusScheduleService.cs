using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.BusSchedul;
using pentasharp.ViewModel.BusReservation;
using pentasharp.Models.Enums;

namespace pentasharp.Services
{
    public class BusScheduleService : IBusScheduleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BusScheduleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ValidateCompanyUser(int companyId)
        {
            return await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId &&
                u.BusinessType == BusinessType.BusCompany);
        }

        public async Task<bool> CheckIfRouteExists(AddRouteViewModel model)
        {
            return await _context.BusRoutes.AnyAsync(r =>
                r.FromLocation == model.FromLocation &&
                r.ToLocation == model.ToLocation);
        }

        public async Task AddRoute(AddRouteViewModel model, int hours, int minutes, int companyId)
        {
            var route = _mapper.Map<BusRoutes>(model);
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);
            route.CreatedAt = DateTime.Now;
            route.BusCompanyId = companyId;

            _context.BusRoutes.Add(route);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AddRouteViewModel>> GetRoutes(int companyId)
        {
            var routes = await _context.BusRoutes
                .Where(b => b.BusCompanyId == companyId)
                .ToListAsync();

            return _mapper.Map<List<AddRouteViewModel>>(routes);
        }

        public async Task EditRoute(int routeId, AddRouteViewModel model, int hours, int minutes, int companyId)
        {
            var route = await _context.BusRoutes.FindAsync(routeId);
            if (route == null) throw new Exception("Route not found.");

            var isDuplicate = await _context.BusRoutes
                .AnyAsync(r => r.RouteId != routeId &&
                               r.FromLocation == model.FromLocation &&
                               r.ToLocation == model.ToLocation);
            if (isDuplicate) throw new Exception("Duplicate route details.");

            route.FromLocation = model.FromLocation;
            route.ToLocation = model.ToLocation;
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoute(int routeId, int companyId)
        {
            var route = await _context.BusRoutes.SingleOrDefaultAsync(r => r.RouteId == routeId);
            if (route == null) throw new Exception("Route not found.");

            _context.BusRoutes.Remove(route);
            await _context.SaveChangesAsync();
        }

        public async Task AddSchedule(AddScheduleViewModel model, int companyId)
        {
            var route = await _context.BusRoutes.SingleOrDefaultAsync(r => r.RouteId == model.RouteId);
            if (route == null) throw new Exception("Route not found.");

            var bus = await _context.Buses.SingleOrDefaultAsync(b => b.BusId == model.BusId);
            if (bus == null) throw new Exception("Bus not found.");

            var duplicateSchedule = await _context.BusSchedules
                .AnyAsync(s => s.BusId == model.BusId && s.DepartureTime == model.DepartureTime);
            if (duplicateSchedule) throw new Exception("Duplicate schedule.");

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            model.AvailableSeats = bus.Capacity;
            model.BusCompanyId = companyId;

            var schedule = _mapper.Map<BusSchedule>(model);
            _context.BusSchedules.Add(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<List<object>> GetSchedules(int companyId)
        {
            return await _context.BusSchedules
                .Where(s => s.BusCompanyId == companyId)
                .Select(s => new
                {
                    s.ScheduleId,
                    s.RouteId,
                    s.BusId,
                    s.DepartureTime,
                    s.ArrivalTime,
                    s.Price,
                    s.AvailableSeats,
                    s.BusCompanyId,
                    Status = s.Status.ToString()
                })
                .ToListAsync<object>();
        }

        public async Task EditSchedule(int scheduleId, AddScheduleViewModel model, int companyId)
        {
            var schedule = await _context.BusSchedules.FindAsync(scheduleId);
            if (schedule == null) throw new Exception("Schedule not found.");

            var isDuplicate = await _context.BusSchedules
                .AnyAsync(s => s.ScheduleId != scheduleId &&
                               s.BusId == model.BusId &&
                               s.DepartureTime == model.DepartureTime);
            if (isDuplicate) throw new Exception("Duplicate schedule.");

            schedule.RouteId = model.RouteId;
            schedule.BusId = model.BusId;
            schedule.DepartureTime = model.DepartureTime;
            schedule.Price = model.Price;
            schedule.AvailableSeats = model.AvailableSeats;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSchedule(int scheduleId, int companyId)
        {
            try
            {
                var schedule = await _context.BusSchedules
                    .SingleOrDefaultAsync(r => r.ScheduleId == scheduleId && r.BusCompanyId == companyId);

                if (schedule == null)
                    throw new KeyNotFoundException("Schedule not found or does not belong to this company.");

                _context.BusSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the entity changes. Details: " + ex.InnerException?.Message, ex);
            }
        }
    }
}