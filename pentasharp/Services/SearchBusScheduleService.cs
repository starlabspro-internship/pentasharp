using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.BusReservation;
using pentasharp.ViewModel.BusSchedul;

namespace pentasharp.Services
{
    public class SearchBusScheduleService : ISearchBusScheduleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SearchBusScheduleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SearchScheduleViewModel>> SearchSchedulesAsync(string from, string to, DateTime date)
        {
            var schedules = await _context.BusSchedules
                .Where(s => EF.Functions.Like(s.Route.FromLocation, $"%{from}%")
                            && EF.Functions.Like(s.Route.ToLocation, $"%{to}%")
                            && s.DepartureTime.Date == date.Date)
                .ProjectTo<SearchScheduleViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return schedules;
        }

        public async Task<string[]> GetFromLocationSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty.");

            var suggestions = await _context.BusRoutes
                .Where(r => EF.Functions.Like(r.FromLocation, $"{query}%"))
                .Select(r => r.FromLocation)
                .Distinct()
                .Take(10)
                .ToArrayAsync();

            return suggestions;
        }

        public async Task<string[]> GetToLocationSuggestionsAsync(string fromLocation, string query)
        {
            if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(fromLocation))
                throw new ArgumentException("Query or FromLocation cannot be null or empty.");

            var suggestions = await _context.BusRoutes
                .Where(r => r.FromLocation == fromLocation && EF.Functions.Like(r.ToLocation, $"{query}%"))
                .Select(r => r.ToLocation)
                .Distinct()
                .Take(10)
                .ToArrayAsync();

            return suggestions;
        }

        public async Task<int> AddReservationAsync(AddBusReservationViewModel model)
        {
            var schedule = await _context.BusSchedules
                .Where(s => s.ScheduleId == model.ScheduleId)
                .Select(s => new
                {
                    s.ScheduleId,
                    s.BusCompanyId,
                    s.AvailableSeats,
                    s.Status
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
                throw new KeyNotFoundException("The schedule does not exist.");

            if (schedule.Status != BusScheduleStatus.Scheduled)
                throw new InvalidOperationException("The schedule is not active for reservations.");

            if (schedule.AvailableSeats < model.NumberOfSeats)
                throw new InvalidOperationException("Not enough seats available for the selected schedule.");

            var reservation = _mapper.Map<BusReservations>(model);
            _context.BusReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation.ReservationId;
        }
    }
}