using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.BusReservation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace pentasharp.Services
{
    public class BusReservationService : IBusReservationService
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticateService _authService;

        public BusReservationService(AppDbContext context, IAuthenticateService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<object>> GetReservationsAsync()
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                throw new UnauthorizedAccessException("No user is logged in or no associated company found.");
            }

            var reservations = await _context.BusReservations
                .Where(r => r.Schedule != null && r.Schedule.Route != null && r.Schedule.Bus != null && r.BusCompanyId == companyId.Value)
                .Select(r => new
                {
                    r.ReservationId,
                    r.ReservationDate,
                    r.NumberOfSeats,
                    r.TotalAmount,
                    r.PaymentStatus,
                    r.Status,
                    User = new
                    {
                        r.User.FirstName,
                        r.User.LastName
                    },
                    Schedule = new
                    {
                        r.Schedule.ScheduleId,
                        r.Schedule.DepartureTime,
                        r.Schedule.ArrivalTime,
                        r.Schedule.Price,
                        r.Schedule.AvailableSeats,
                        r.Schedule.Bus.BusNumber,
                        r.Schedule.Route.FromLocation,
                        r.Schedule.Route.ToLocation
                    }
                })
                .ToListAsync();

            return reservations.Cast<object>().ToList();
        }

        public async Task<object> ConfirmReservationAsync(EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
                throw new ArgumentException("Invalid request. Reservation ID is required.");

            var reservation = await _context.BusReservations
                .Where(r => r.ReservationId == model.ReservationId)
                .FirstOrDefaultAsync();

            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            if (reservation.Status == BusReservationStatus.Confirmed)
                throw new InvalidOperationException("Reservation is already confirmed.");

            var scheduleToUpdate = await _context.BusSchedules.FindAsync(reservation.ScheduleId);

            if (scheduleToUpdate != null)
            {
                if (scheduleToUpdate.AvailableSeats < reservation.NumberOfSeats)
                    throw new InvalidOperationException("Not enough available seats to confirm this reservation.");

                scheduleToUpdate.AvailableSeats -= reservation.NumberOfSeats;
            }

            reservation.Status = BusReservationStatus.Confirmed;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new
            {
                reservation.ReservationId,
                reservation.Status,
                reservation.UpdatedAt
            };
        }

        public async Task<object> CancelReservationAsync(EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
                throw new ArgumentException("Invalid request. Reservation ID is required.");

            var reservation = await _context.BusReservations
                .Where(r => r.ReservationId == model.ReservationId)
                .FirstOrDefaultAsync();

            if (reservation == null)
                throw new KeyNotFoundException("Reservation not found.");

            var scheduleToUpdate = await _context.BusSchedules.FindAsync(reservation.ScheduleId);

            if (scheduleToUpdate != null)
                scheduleToUpdate.AvailableSeats += reservation.NumberOfSeats;

            reservation.Status = BusReservationStatus.Canceled;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new
            {
                reservation.ReservationId,
                reservation.Status,
                reservation.UpdatedAt
            };
        }
    }
}