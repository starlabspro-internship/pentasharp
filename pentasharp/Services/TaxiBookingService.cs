using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pentasharp.Data;
using pentasharp.Interfaces;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using pentasharp.ViewModel.TaxiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pentasharp.Services
{
    public class TaxiBookingService : ITaxiBookingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaxiReservationService> _logger;

        public TaxiBookingService(AppDbContext context, IMapper mapper, ILogger<TaxiReservationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TaxiCompanyViewModel>> GetAllCompaniesAsync()
        {
            var companies = await _context.TaxiCompanies.ToListAsync();
            return _mapper.Map<List<TaxiCompanyViewModel>>(companies);
        }

        public async Task<bool> CreateBookingAsync(TaxiBookingRequest request)
        {
            var taxiBooking = new TaxiBookings
            {
                TaxiCompanyId = request.TaxiCompanyId,
                PickupLocation = request.PickupLocation,
                DropoffLocation = request.DropoffLocation,
                BookingTime = request.BookingTime,
                PassengerCount = request.PassengerCount,
                UserId = request.UserId,
                Status = request.Status
            };

            _context.TaxiBookings.Add(taxiBooking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaxiBookingViewModel>> GetAllBookingsAsync()
        {
            var bookings = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.TaxiCompany)
                .Include(b => b.Taxi)
                .ToListAsync();

            return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
        }

        public async Task<TaxiBookingViewModel> GetBookingByIdAsync(int id)
        {
            var booking = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.Taxi)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return null;
            }

            return _mapper.Map<TaxiBookingViewModel>(booking);
        }

        public async Task<bool> UpdateBookingAsync(EditTaxiBookingViewModel model)
        {
            var booking = await _context.TaxiBookings.FirstOrDefaultAsync(b => b.BookingId == model.BookingId);

            if (booking == null)
            {
                return false;
            }

            booking.PickupLocation = model.PickupLocation;
            booking.DropoffLocation = model.DropoffLocation;
            booking.BookingTime = model.BookingTime;

            if (Enum.TryParse(typeof(ReservationStatus), model.Status.ToString(), true, out var status))
            {
                booking.Status = (ReservationStatus)status;
            }
            else
            {
                return false;
            }

            booking.TaxiId = model.TaxiId;
            booking.UpdatedAt = model.UpdateAt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaxiBookingViewModel>> GetBookingsForUserAsync(int userId)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
                if (!userExists)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                var bookings = await _context.TaxiBookings
                    .Include(r => r.User)
                    .Include(r => r.Taxi)
                    .Include(r => r.TaxiCompany)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();

                return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching bookings for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId, int userId)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
                if (!userExists)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return false; 
                }
                var booking = await _context.TaxiBookings
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.UserId == userId && b.Status == ReservationStatus.Pending);

                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {BookingId} not found, does not belong to user {UserId}, or is not pending", bookingId, userId);
                    return false;
                }

                _context.TaxiBookings.Remove(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Booking with ID {BookingId} canceled successfully", bookingId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while canceling booking with ID {BookingId}.", bookingId);
                throw;
            }
        }

    }
}