using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;
using pentasharp.Interfaces;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "TaxiCompany" })]
    [Route("Driver")]
    public class TaxiDriverController : Controller
    {
        private readonly IDriverDashboardService _driverDashboardService;

        public TaxiDriverController(IDriverDashboardService driverDashboardService)
        {
            _driverDashboardService = driverDashboardService;
        }
        public IActionResult TaxiDriver()
        {
            return View();
        }

        [HttpGet("Bookings")]
        public async Task<IActionResult> GetAvailableBookings()
        {
            var bookings = await _driverDashboardService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpPost("ClaimBooking/{bookingId}")]
        public async Task<IActionResult> ClaimBooking(int bookingId)
        {

            if (bookingId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid booking ID." });
            }

            var result = await _driverDashboardService.ClaimBookingAsync(bookingId);

            if (result)
                return Ok(new { success = true, message = "Booking claimed successfully." });

            return BadRequest(new { success = false, message = "Unable to claim booking." });
        }

        [HttpPost("StartTrip/{bookingId}")]
        public async Task<IActionResult> StartTrip(int bookingId)
        {
            try
            {
                var result = await _driverDashboardService.StartTripAsync(bookingId);

                if (!result.Success)
                {
                    return BadRequest(new { message = result.Message });
                }

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("EndTrip/{bookingId}")]
        public async Task<IActionResult> EndTrip(int bookingId, [FromBody] decimal fare)
        {
            var result = await _driverDashboardService.EndTripAsync(bookingId, fare);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                bookingId = bookingId,
                status = result.Booking.Status,
                tripEndTime = result.Booking.TripEndTime,
                fare = result.Booking.Fare
            });
        }

        [HttpGet("Reservations")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservations = await _driverDashboardService.GetReservationsAsync();
                return Ok(new { success = true, data = reservations });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while fetching reservations.", error = ex.Message });
            }
        }

        [HttpPost("AcceptReservation/{reservationId}")]
        public async Task<IActionResult> AcceptReservation(int reservationId)
        {
            if (reservationId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid reservation ID." });
            }

            var result = await _driverDashboardService.AcceptReservationAsync(reservationId);

            if (result)
            {
                return Ok(new { success = true, message = "Reservation accepted successfully." });
            }

            return BadRequest(new { success = false, message = "Unable to accept reservation." });
        }

        [HttpPost("StartTripReservation/{reservationId}")]
        public async Task<IActionResult> StartTripReservation(int reservationId)
        {
            var result = await _driverDashboardService.StartReservationTripAsync(reservationId);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }

        [HttpPost("EndReservationTrip/{reservationId}")]
        public async Task<IActionResult> EndReservationTrip(int reservationId)
        {
            var result = await _driverDashboardService.EndReservationTripAsync(reservationId);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }
    }
}