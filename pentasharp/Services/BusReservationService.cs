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

        public BusReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<object>> GetReservationsAsync(int companyId)
        {
            var reservations = await _context.BusReservations
                .Where(r => r.Schedule != null && r.Schedule.Route != null && r.Schedule.Bus != null && r.BusCompanyId == companyId)
                .Select(r => new
                {
                    ReservationId = r.ReservationId,
                    ReservationDate = r.ReservationDate,
                    NumberOfSeats = r.NumberOfSeats,
                    TotalAmount = r.TotalAmount,
                    PaymentStatus = r.PaymentStatus,
                    Status = r.Status,
                    User = new
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName
                    },
                    Schedule = new
                    {
                        ScheduleId = r.Schedule.ScheduleId,
                        DepartureTime = r.Schedule.DepartureTime,
                        ArrivalTime = r.Schedule.ArrivalTime,
                        Price = r.Schedule.Price,
                        AvailableSeats = r.Schedule.AvailableSeats,
                        BusNumber = r.Schedule.Bus.BusNumber,
                        FromLocation = r.Schedule.Route.FromLocation,
                        ToLocation = r.Schedule.Route.ToLocation
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