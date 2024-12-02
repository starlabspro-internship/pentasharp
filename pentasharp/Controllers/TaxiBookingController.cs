using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.DTOs;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.Models.Utilities;
using pentasharp.ViewModel.Taxi;
using pentasharp.ViewModel.TaxiBooking;
using pentasharp.ViewModel.TaxiModels;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiBooking")]
    public class TaxiBookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaxiBookingController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("SearchAvailableTaxis")]
        public IActionResult GetCompanies()
        {
            var companies = _context.TaxiCompanies.ToList();
            var viewModel = _mapper.Map<List<TaxiCompanyViewModel>>(companies);
            return Ok(new { success = true, companies = viewModel });
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            var taxiBooking = _mapper.Map<TaxiBookings>(model);

            _context.TaxiBookings.Add(taxiBooking);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Booking created successfully" });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.TaxiCompany)
                .Include(b => b.Taxi)
                .ToListAsync();

            var viewModel = _mapper.Map<List<TaxiBookingViewModel>>(bookings);

            return Ok(new StandardApiResponse<List<TaxiBookingViewModel>>
            {
                Success = true,
                Message = ResponseMessages.Success,
                Code = ResponseCodes.Success,
                Data = viewModel
            });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetBooking")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _context.TaxiBookings
                .Include(b => b.User)
                .Include(b => b.Taxi)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound(new StandardApiResponse<string>
                {
                    Success = false,
                    Message = ResponseMessages.NotFound,
                    Code = ResponseCodes.NotFound
                });
            }

            var viewModel = new TaxiBookingViewModel
            {
                BookingId = booking.BookingId,
                PassengerName = booking.User != null ? $"{booking.User.FirstName} {booking.User.LastName}" : "Unknown",
                PickupLocation = booking.PickupLocation,
                DropoffLocation = booking.DropoffLocation,
                BookingTime = booking.BookingTime.ToString("HH:mm"),
                Status = booking.Status.ToString(),
                DriverName = booking.Taxi?.DriverName ?? "No Driver Assigned",
                TaxiId = booking.Taxi?.TaxiId
            };


            return Ok(new StandardApiResponse<TaxiBookingViewModel>
            {
                Success = true,
                Message = ResponseMessages.Success,
                Code = ResponseCodes.Success,
                Data = viewModel
            });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] EditTaxiBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data." });

            var booking = await _context.TaxiBookings.FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
            if (booking == null)
                return NotFound(new { success = false, message = "Booking not found." });

            booking.PickupLocation = model.PickupLocation;
            booking.DropoffLocation = model.DropoffLocation;
            booking.BookingTime = model.BookingTime;
            if (Enum.TryParse(typeof(ReservationStatus), model.Status.ToString(), true, out var status))
                booking.Status = (ReservationStatus)status;
            else
                return BadRequest(new { success = false, message = "Invalid status value." });

            booking.TaxiId = model.TaxiId;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Booking updated successfully." });
        }
    }
}