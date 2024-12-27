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
            {
                _logger.LogWarning("Attempt to fetch bookings failed: driver is not authenticated.");
                return new List<TaxiBookingViewModel>();
            }

            try
            {
                _logger.LogInformation("Fetching bookings for driver ID: {DriverId}", driverId);

                var bookings = await _context.TaxiBookings
                    .Where(b => b.Status == ReservationStatus.Pending)
                    .Include(b => b.User)
                    .Include(b => b.Taxi)
                    .ThenInclude(t => t.Driver)
                    .Where(t => t.DriverId == driverId.Value)
                    .ToListAsync();

                _logger.LogInformation("Successfully fetched {Count} bookings for driver ID: {DriverId}", bookings.Count, driverId);

                return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bookings for driver ID: {DriverId}", driverId);
                return new List<TaxiBookingViewModel>();
            }
        }
        public async Task<bool> ClaimBookingAsync(int bookingId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                _logger.LogWarning("Attempt to claim booking failed: driver is not authenticated.");
                return false;
            }

            try
            {
                _logger.LogInformation("Driver ID: {DriverId} attempting to claim booking ID: {BookingId}", driverId, bookingId);

                var booking = await _context.TaxiBookings
                    .Include(b => b.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Status == ReservationStatus.Pending);

                if (booking == null)
                {
                    _logger.LogWarning("Booking ID: {BookingId} not found or not in pending status for driver ID: {DriverId}", bookingId, driverId);
                    return false;
                }

                if (booking.Taxi.DriverId != driverId.Value)
                {
                    _logger.LogWarning("Driver ID: {DriverId} is not authorized to claim booking ID: {BookingId}", driverId, bookingId);
                    return false;
                }

                booking.Status = ReservationStatus.Confirmed;
                _context.TaxiBookings.Update(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Driver ID: {DriverId} successfully claimed booking ID: {BookingId}", driverId, bookingId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while driver ID: {DriverId} was claiming booking ID: {BookingId}", driverId, bookingId);
                return false;
            }
        }

        public async Task<(bool Success, string Message)> StartTripAsync(int bookingId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                _logger.LogWarning("Attempt to start trip failed: Driver is not authenticated.");
                return (false, "Driver is not authenticated.");
            }

            try
            {
                _logger.LogInformation("Driver ID: {DriverId} attempting to start trip for booking ID: {BookingId}", driverId, bookingId);

                var booking = await _context.TaxiBookings
                    .Include(b => b.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                {
                    _logger.LogWarning("Booking ID: {BookingId} not found for driver ID: {DriverId}", bookingId, driverId);
                    return (false, "Booking not found.");
                }

                if (booking.Taxi.DriverId != driverId.Value)
                {
                    _logger.LogWarning("Driver ID: {DriverId} is not authorized to start trip for booking ID: {BookingId}", driverId, bookingId);
                    return (false, "You are not authorized to start this trip.");
                }

                if (booking.Status != ReservationStatus.Confirmed)
                {
                    _logger.LogWarning("Booking ID: {BookingId} cannot be started as its status is not 'Confirmed'. Current status: {Status}", bookingId, booking.Status);
                    return (false, "Only confirmed bookings can be started.");
                }

                booking.Status = ReservationStatus.InProgress;
                booking.TripStartTime = DateTime.Now;

                _context.TaxiBookings.Update(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Trip successfully started for booking ID: {BookingId} by driver ID: {DriverId}", bookingId, driverId);
                return (true, "Trip started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while driver ID: {DriverId} was starting trip for booking ID: {BookingId}", driverId, bookingId);
                return (false, "An error occurred while starting the trip.");
            }
        }

        public async Task<(bool Success, string Message, TaxiBookingViewModel Booking)> EndTripAsync(int bookingId, decimal fare)
        {
            try
            {
                var driverId = _authService.GetCurrentUserId();

                if (driverId == null)
                {
                    _logger.LogWarning("Attempt to end trip failed: Driver is not authenticated.");
                    return (false, "Driver is not authenticated.", null);
                }

                _logger.LogInformation("Driver ID: {DriverId} attempting to end trip for booking ID: {BookingId}", driverId, bookingId);

                var booking = await _context.TaxiBookings
                    .Include(b => b.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                {
                    _logger.LogWarning("Booking ID: {BookingId} not found for driver ID: {DriverId}", bookingId, driverId);
                    return (false, "Booking not found.", null);
                }

                if (booking.Taxi.DriverId != driverId.Value)
                {
                    _logger.LogWarning("Driver ID: {DriverId} is not authorized to end trip for booking ID: {BookingId}", driverId, bookingId);
                    return (false, "You are not authorized to end this trip.", null);
                }

                if (booking.Status != ReservationStatus.InProgress)
                {
                    _logger.LogWarning("Booking ID: {BookingId} cannot be ended as its status is not 'InProgress'. Current status: {Status}", bookingId, booking.Status);
                    return (false, "Trip is not in progress and cannot be ended.", null);
                }

                booking.Status = ReservationStatus.End;
                booking.TripEndTime = DateTime.Now;
                booking.Fare = fare;

                _context.TaxiBookings.Update(booking);
                await _context.SaveChangesAsync();

                var bookingViewModel = _mapper.Map<TaxiBookingViewModel>(booking);

                _logger.LogInformation("Trip successfully ended for booking ID: {BookingId} by driver ID: {DriverId}. Fare: {Fare}, End Time: {EndTime}", bookingId, driverId, fare, booking.TripEndTime);
                return (true, "Trip ended successfully.", bookingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while driver ID: {DriverId} was attempting to end trip for booking ID: {BookingId}", _authService.GetCurrentUserId(), bookingId);
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<List<TaxiReservationRequest>> GetReservationsAsync()
        {
            try
            {
                var driverId = _authService.GetCurrentUserId();

                if (driverId == null)
                {
                    _logger.LogWarning("Failed to fetch reservations: Driver is not authenticated.");
                    return new List<TaxiReservationRequest>();
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching reservations for driver ID: {DriverId}", _authService.GetCurrentUserId());
                return new List<TaxiReservationRequest>();
            }
        }

        public async Task<bool> AcceptReservationAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                _logger.LogWarning("Failed to accept reservation {ReservationId}: Driver is not authenticated.", reservationId);
                return false;
            }

            try
            {
                var reservation = await _context.TaxiReservations
                    .Include(r => r.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.Status == ReservationStatus.Pending);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation {ReservationId} not found or is not in a pending state.", reservationId);
                    return false;
                }

                if (reservation.Taxi.DriverId != driverId.Value)
                {
                    _logger.LogWarning("Driver {DriverId} is not authorized to accept reservation {ReservationId}.", driverId, reservationId);
                    return false;
                }

                reservation.Status = ReservationStatus.Confirmed;

                _context.TaxiReservations.Update(reservation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation {ReservationId} was successfully accepted by driver {DriverId}.", reservationId, driverId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accepting reservation {ReservationId} by driver {DriverId}.", reservationId, driverId);
                return false;
            }
        }

        public async Task<(bool Success, string Message)> StartReservationTripAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                _logger.LogWarning("Failed to start trip for reservation {ReservationId}: Driver is not authenticated.", reservationId);
                return (false, "Driver is not authenticated.");
            }

            try
            {
                var reservation = await _context.TaxiReservations
                    .Include(r => r.Taxi)
                    .ThenInclude(t => t.Driver)
                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation {ReservationId} not found.", reservationId);
                    return (false, "Reservation not found.");
                }

                if (reservation.Taxi.DriverId != driverId.Value)
                {
                    _logger.LogWarning("Driver {DriverId} is not authorized to start the trip for reservation {ReservationId}.", driverId, reservationId);
                    return (false, "You are not authorized to start this trip.");
                }

                if (reservation.Status != ReservationStatus.Confirmed)
                {
                    _logger.LogWarning("Reservation {ReservationId} is not in a confirmed state and cannot be started.", reservationId);
                    return (false, "Only confirmed reservations can be started.");
                }

                reservation.Status = ReservationStatus.InProgress;
                reservation.TripStartTime = DateTime.Now;

                _context.TaxiReservations.Update(reservation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Trip for reservation {ReservationId} successfully started by driver {DriverId}.", reservationId, driverId);
                return (true, "Trip started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while starting the trip for reservation {ReservationId} by driver {DriverId}.", reservationId, driverId);
                return (false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EndReservationTripAsync(int reservationId)
        {
            var driverId = _authService.GetCurrentUserId();

            if (driverId == null)
            {
                _logger.LogWarning("Driver is not authenticated. Reservation ID: {ReservationId}", reservationId);
                return (false, "Driver is not authenticated.");
            }

            var reservation = await _context.TaxiReservations
                .Include(r => r.Taxi)
                .ThenInclude(t => t.Driver)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                _logger.LogWarning("Reservation not found. Reservation ID: {ReservationId}", reservationId);
                return (false, "Reservation not found.");
            }

            if (reservation.Taxi.DriverId != driverId.Value)
            {
                _logger.LogWarning("Driver ID mismatch. Driver {DriverId} attempted to end trip for reservation ID: {ReservationId}", driverId.Value, reservationId);
                return (false, "You are not authorized to end this trip.");
            }

            if (reservation.Status != ReservationStatus.InProgress)
            {
                _logger.LogWarning("Trip not in progress. Reservation ID: {ReservationId}, Current Status: {Status}", reservationId, reservation.Status);
                return (false, "Trip is not in progress and cannot be ended.");
            }

            reservation.Status = ReservationStatus.End;
            reservation.TripEndTime = DateTime.Now;

            _context.TaxiReservations.Update(reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trip ended successfully. Reservation ID: {ReservationId}, Driver ID: {DriverId}, End Time: {EndTime}",
                reservationId, driverId.Value, reservation.TripEndTime);

            return (true, "Trip ended successfully.");
        }
    }
}