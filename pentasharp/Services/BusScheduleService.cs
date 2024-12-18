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
        private readonly IAuthenticateService _authService;

        public BusScheduleService(AppDbContext context, IMapper mapper, IAuthenticateService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<bool> CheckIfRouteExists(AddRouteViewModel model)
        {
            return await _context.BusRoutes.AnyAsync(r =>
                r.FromLocation == model.FromLocation &&
                r.ToLocation == model.ToLocation);
        }

        public async Task<bool> AddRoute(AddRouteViewModel model, int hours, int minutes)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var route = _mapper.Map<BusRoutes>(model);
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);
            route.CreatedAt = DateTime.Now;
            route.BusCompanyId = companyId.Value;

            try
            {
                _context.BusRoutes.Add(route);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<AddRouteViewModel>> GetRoutes()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var routes = await _context.BusRoutes
                .Where(b => b.BusCompanyId == companyId.Value)
                .ToListAsync();

            return _mapper.Map<List<AddRouteViewModel>>(routes);
        }

        public async Task<bool> EditRoute(int routeId, AddRouteViewModel model, int hours, int minutes)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var route = await _context.BusRoutes.FindAsync(routeId);
            if (route == null) return false;

            var isDuplicate = await _context.BusRoutes
                .AnyAsync(r => r.RouteId != routeId &&
                               r.FromLocation == model.FromLocation &&
                               r.ToLocation == model.ToLocation &&
                               r.BusCompanyId == companyId.Value);

            if (isDuplicate) return false;

            route.FromLocation = model.FromLocation;
            route.ToLocation = model.ToLocation;
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoute(int routeId)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var route = await _context.BusRoutes.SingleOrDefaultAsync(r => r.RouteId == routeId && r.BusCompanyId == companyId.Value);
            if (route == null) return false;

            _context.BusRoutes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSchedule(AddScheduleViewModel model)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var route = await _context.BusRoutes.SingleOrDefaultAsync(r => r.RouteId == model.RouteId);
            if (route == null)
            {
                throw new KeyNotFoundException($"Route with ID {model.RouteId} was not found.");
            }

            var bus = await _context.Buses.SingleOrDefaultAsync(b => b.BusId == model.BusId);
            if (bus == null)
            {
                throw new KeyNotFoundException($"Bus with ID {model.BusId} was not found.");
            }

            var duplicateSchedule = await _context.BusSchedules
                .AnyAsync(s => s.BusId == model.BusId && s.DepartureTime == model.DepartureTime);

            if (duplicateSchedule)
            {
                throw new InvalidOperationException("A schedule with the same bus and departure time already exists.");
            }

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            model.AvailableSeats = bus.Capacity;
            model.BusCompanyId = companyId.Value;

            var schedule = _mapper.Map<BusSchedule>(model);

            try
            {
                _context.BusSchedules.Add(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the schedule.", ex);
            }
        }

        public async Task<List<object>> GetSchedules()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            return await _context.BusSchedules
                .Where(s => s.BusCompanyId == companyId.Value)
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

        public async Task<bool> EditSchedule(int scheduleId, AddScheduleViewModel model)
        {
            var schedule = await _context.BusSchedules.FindAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} was not found.");

            var isDuplicate = await _context.BusSchedules
                .AnyAsync(s => s.ScheduleId != scheduleId &&
                               s.BusId == model.BusId &&
                               s.DepartureTime == model.DepartureTime);
            if (isDuplicate)
                throw new InvalidOperationException("A schedule with the same bus and departure time already exists.");

            schedule.RouteId = model.RouteId;
            schedule.BusId = model.BusId;
            schedule.DepartureTime = model.DepartureTime;
            schedule.Price = model.Price;
            schedule.AvailableSeats = model.AvailableSeats;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSchedule(int scheduleId)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var schedule = await _context.BusSchedules
                .SingleOrDefaultAsync(r => r.ScheduleId == scheduleId && r.BusCompanyId == companyId.Value);

            if (schedule == null) return false;

            _context.BusSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}