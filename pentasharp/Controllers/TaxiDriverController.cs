using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiDriver;
using pentasharp.ViewModel.TaxiModels;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [ServiceFilter(typeof(AdminOnlyFilter))]
    [Route("api/TaxiDriver")]
    public class TaxiDriverController : Controller
    {
        private readonly AppDbContext _context;

        public TaxiDriverController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult TaxiDriver()
        {
            return View();
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.TaxiBookings
                .Where(b => b.Status.ToString() == "Pending")
                .Select(b => new TaxiBookingViewModel
                {
                    BookingId = b.BookingId,
                    PassengerName = $"{b.User.FirstName} {b.User.LastName}",
                    PickupLocation = b.PickupLocation,
                    DropoffLocation = b.DropoffLocation,
                    BookingTime = b.BookingTime.ToString("HH:mm"),
                    Status = b.Status.ToString(),
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpPost("StartTrip")]
        public async Task<IActionResult> StartTrip([FromBody] StartTripViewModel model)
        {
            var booking = await _context.TaxiBookings.FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
            if (booking == null) return NotFound("Booking not found.");

            booking.Status = ReservationStatus.InProgress;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Trip started successfully." });
        }

        [HttpPost("EndTrip")]
        public async Task<IActionResult> EndTrip([FromBody] EndTripViewModel model)
        {
            var booking = await _context.TaxiBookings.FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
            if (booking == null) return NotFound("Booking not found.");

            booking.Status = ReservationStatus.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            booking.Fare = model.Fare;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Trip ended successfully." });
        }
    }
}
