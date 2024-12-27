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
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace pentasharp.Services
{
    public class TaxiBookingService : ITaxiBookingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaxiReservationService> _logger;
        private readonly IAuthenticateService _authService;

        public TaxiBookingService(AppDbContext context, IMapper mapper, ILogger<TaxiReservationService> logger, IAuthenticateService authService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
        }

        public async Task<List<TaxiCompanyViewModel>> GetAllCompaniesAsync()
        {
            var companies = await _context.TaxiCompanies
                .Where(tc => !tc.IsDeleted)
                .ToListAsync();

            return _mapper.Map<List<TaxiCompanyViewModel>>(companies);
        }

        public async Task<bool> CreateBookingAsync(CreateBookingViewModel model)
        {
            _logger.LogInformation("Starting booking creation process for user ID: {UserId}", model.UserId);

            var taxiBooking = new TaxiBookings
            {
                TaxiCompanyId = model.TaxiCompanyId,
                PickupLocation = model.PickupLocation,
                DropoffLocation = model.DropoffLocation,
                BookingTime = model.BookingTime,
                PassengerCount = model.PassengerCount,
                UserId = model.UserId,
                Status = ReservationStatus.Pending
            };

            try
            {
                _context.TaxiBookings.Add(taxiBooking);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Booking successfully saved to the database for user ID: {UserId}", model.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a booking for user ID: {UserId}", model.UserId);
                return false;
            }
        }

        public async Task<List<TaxiBookingViewModel>> GetAllBookingsAsync()
        {
            var bookings = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.Taxi)
                .ThenInclude(t => t.Driver)
                .Include(b => b.TaxiCompany)
                .ToListAsync();

            return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
        }

        public async Task<TaxiBookingViewModel> GetBookingByIdAsync(int id)
        {
            var booking = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.Taxi)
                .ThenInclude(t => t.Driver)
                 .Where(b => _context.Users
                    .Any(u => u.CompanyId == b.TaxiCompanyId))
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

            var taxi = await _context.Taxis.FirstOrDefaultAsync(t=>t.TaxiId == model.TaxiId);
            if (taxi != null)
            {
                booking.DriverId = taxi.DriverId;
            }
            else
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaxiBookingViewModel>> GetBookingsForUserAsync()
        {
            var userId = _authService.GetCurrentUserId();

            if (!userId.HasValue)
            {
                _logger.LogWarning("User ID is null or not found.");
                throw new KeyNotFoundException("User ID not found.");
            }

            try
            {
                var bookings = await _context.TaxiBookings
                    .Include(r => r.User)
                    .Include(r => r.Taxi)
                    .ThenInclude(t => t.Driver)
                    .Include(r => r.TaxiCompany)
                    .Where(r => r.UserId == userId.Value)
                    .ToListAsync();

                if (!bookings.Any()) 
                {
                    _logger.LogWarning("No bookings found for user with ID {UserId}.", userId.Value);
                     return new List<TaxiBookingViewModel>();
                }

                return _mapper.Map<List<TaxiBookingViewModel>>(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching bookings for user with ID {UserId}.", userId.Value);
                throw;
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var companyId = _authService.GetCurrentCompanyId();

            if (!companyId.HasValue)
            {
                _logger.LogWarning("Company ID is null or not found.");
                throw new KeyNotFoundException("Company ID not found.");
            }

            try
            {
                var booking = await _context.TaxiBookings
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.TaxiCompanyId == companyId && b.Status == ReservationStatus.Pending);

                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {BookingId} not found, does not belong to user {UserId}, or is not pending", bookingId);
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