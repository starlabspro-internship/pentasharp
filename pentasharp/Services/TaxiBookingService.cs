using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public TaxiBookingService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
    }
}