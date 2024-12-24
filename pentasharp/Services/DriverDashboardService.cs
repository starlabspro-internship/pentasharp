using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using Microsoft.Extensions.Logging;


namespace pentasharp.Services
{

    public class DriverDashboardService : IDriverDashboardService
    {
        private readonly IAuthenticateService _authService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DriverDashboardService> _logger;

        public DriverDashboardService(AppDbContext context, IAuthenticateService authService, IMapper mapper, ILogger<DriverDashboardService> logger)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TaxiBookingViewModel>> GetAllBookingsAsync()
        {

            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
                return new List<TaxiBookingViewModel>();

            var bookings = await _context.TaxiBookings
            .Where(b => b.Status == ReservationStatus.Pending)
                .Include(b => b.User)
                .Include(b => b.Taxi)
                .ThenInclude(t => t.Driver)
                .Where(t => t.DriverId == driverId.Value)
                .ToListAsync();

            return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
        }

        public async Task<bool> ClaimBookingAsync(int bookingId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
                return false;

            var booking = await _context.TaxiBookings
                .Include(b => b.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Status == ReservationStatus.Pending);

            if (booking == null || booking.Taxi.DriverId != driverId.Value)
                return false;
            booking.Status = ReservationStatus.Confirmed;
            _context.TaxiBookings.Update(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, string Message)> StartTripAsync(int bookingId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                return (false, "Driver is not authenticated.");
            }

            var booking = await _context.TaxiBookings
                .Include(b => b.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return (false, "Booking not found.");
            }

            if (booking.Taxi.DriverId != driverId.Value)
            {
                return (false, "You are not authorized to start this trip.");
            }

            if (booking.Status != ReservationStatus.Confirmed)
            {
                return (false, "Only confirmed bookings can be started.");
            }

            booking.Status = ReservationStatus.InProgress;
            booking.TripStartTime = DateTime.Now;

            _context.TaxiBookings.Update(booking);
            await _context.SaveChangesAsync();

            return (true, "Trip started successfully.");
        }

        public async Task<(bool Success, string Message, TaxiBookingViewModel Booking)> EndTripAsync(int bookingId, decimal fare)
        {
            try
            {
                var driverId = _authService.GetCurrentUserId();

                if (driverId == null)
                {
                    return (false, "Driver is not authenticated.", null);
                }

                var booking = await _context.TaxiBookings
                    .Include(b => b.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                {
                    return (false, "Booking not found.", null);
                }

                if (booking.Taxi.DriverId != driverId.Value)
                {
                    return (false, "You are not authorized to end this trip.", null);
                }

                if (booking.Status != ReservationStatus.InProgress)
                {
                    return (false, "Trip is not in progress and cannot be ended.", null);
                }
                booking.Status = ReservationStatus.End;
                booking.TripEndTime = DateTime.Now;
                booking.Fare = fare;

                _context.TaxiBookings.Update(booking);
                await _context.SaveChangesAsync();

                var bookingViewModel = _mapper.Map<TaxiBookingViewModel>(booking);

                return (true, "Trip ended successfully.", bookingViewModel);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<List<TaxiReservationRequest>> GetReservationsAsync()
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
                return new List<TaxiReservationRequest>();

            var reservations = await _context.TaxiReservations.Where(r => r.Status == ReservationStatus.Pending)
                .Include(r => r.User)
                .Include(r => r.Taxi)
                .ThenInclude(t => t.Driver)
                .Include(r => r.TaxiCompany)
                .Where(t => t.Taxi.DriverId == driverId.Value)
                .Select(r => new TaxiReservationRequest
                {

                    ReservationId = r.ReservationId,
                    PassengerName = $"{r.User.FirstName} {r.User.LastName}".Trim() ?? "Unknown",
                    DriverName = r.Taxi != null
                        ? $"{r.Taxi.Driver.FirstName} {r.Taxi.Driver.LastName}".Trim()
                        : "Unassigned",
                    PickupLocation = r.PickupLocation,
                    DropoffLocation = r.DropoffLocation,
                    ReservationDate = r.ReservationTime.ToString("yyyy-MM-dd"),
                    ReservationTime = r.ReservationTime.ToString("hh:mm tt"),
                    Fare = r.Fare.HasValue ? r.Fare.Value : (decimal?)null,

                })
                .ToListAsync();

            _logger.LogInformation("Successfully fetched {Count} reservations for driver ID: {DriverId}", reservations.Count, driverId);
            return reservations;
        }

        public async Task<bool> AcceptReservationAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
                return false;

            var reservation = await _context.TaxiReservations
                .Include(r => r.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.Status == ReservationStatus.Pending);

            if (reservation == null || reservation.Taxi.DriverId != driverId.Value)
                return false;

            reservation.Status = ReservationStatus.Confirmed;

            _context.TaxiReservations.Update(reservation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Success, string Message)> StartReservationTripAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                return (false, "Driver is not authenticated.");
            }

            var reservation = await _context.TaxiReservations
                .Include(r => r.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                return (false, "Reservation not found.");
            }

            if (reservation.Taxi.DriverId != driverId.Value)
            {
                return (false, "You are not authorized to start this trip.");
            }

            if (reservation.Status != ReservationStatus.Confirmed)
            {
                return (false, "Only confirmed reservations can be started.");
            }

            reservation.Status = ReservationStatus.InProgress;
            reservation.TripStartTime = DateTime.Now;

            _context.TaxiReservations.Update(reservation);
            await _context.SaveChangesAsync();

            return (true, "Trip started successfully.");
        }

        public async Task<(bool Success, string Message)> EndReservationTripAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                return (false, "Driver is not authenticated.");
            }

            var reservation = await _context.TaxiReservations
                .Include(r => r.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                return (false, "Reservation not found.");
            }

            if (reservation.Taxi.DriverId != driverId.Value)
            {
                return (false, "You are not authorized to end this trip.");
            }

            if (reservation.Status != ReservationStatus.InProgress)
            {
                return (false, "Trip is not in progress and cannot be ended.");
            }

            reservation.Status = ReservationStatus.End;
            reservation.TripEndTime = DateTime.Now;

            _context.TaxiReservations.Update(reservation);
            await _context.SaveChangesAsync();

            return (true, "Trip ended successfully.");
        }
    }
}