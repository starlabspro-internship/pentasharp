using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Filters;
using Microsoft.AspNetCore.Authorization;
using pentasharp.Interfaces;
using pentasharp.Models.DTOs;
using pentasharp.Models.Enums;
using pentasharp.Models.TaxiRequest;
using pentasharp.Models.Utilities;
using pentasharp.Services;

namespace WebApplication1.Controllers
{
    [TypeFilter(typeof(BusinessOnlyFilter), Arguments = new object[] { "TaxiCompany" })]
    [Route("Business/TaxiManagement")]
    public class TaxiManagementController : Controller
    {
        private readonly ITaxiBookingService _taxiBookingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITaxiReservationService _taxiReservationService;

        public TaxiManagementController(ITaxiBookingService taxiBookingService, IHttpContextAccessor httpContextAccessor, ITaxiReservationService taxiReservationService)
        {
            _taxiBookingService = taxiBookingService;
            _httpContextAccessor = httpContextAccessor;
            _taxiReservationService = taxiReservationService;
        }

        [HttpGet("TaxiBookingManagement")]
        public IActionResult TaxiBookingManagement()
        {
            return View();
        }

        [HttpGet("TaxiReservationManagement")]
        public IActionResult TaxiReservationManagement()
        {
            return View();
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var userId = session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(new StandardResponse(
                    ApiStatusEnum.UNAUTHORIZED,
                    Guid.NewGuid().ToString(),
                    "User is not logged in."
                ));
            }

            var bookings = await _taxiBookingService.GetAllBookingsAsync(userId.Value);

            return Ok(new StandardApiResponse<List<TaxiBookingViewModel>>
            {
                Success = true,
                Message = ResponseMessages.Success,
                Code = ResponseCodes.Success,
                Data = bookings
            });
        }

        [HttpGet("GetBooking")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _taxiBookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new StandardApiResponse<string>
                {
                    Success = false,
                    Message = ResponseMessages.NotFound,
                    Code = ResponseCodes.NotFound
                });
            }

            return Ok(new StandardApiResponse<TaxiBookingViewModel>
            {
                Success = true,
                Message = ResponseMessages.Success,
                Code = ResponseCodes.Success,
                Data = booking
            });
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] EditTaxiBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data." });

            var success = await _taxiBookingService.UpdateBookingAsync(model);
            if (!success)
                return NotFound(new { success = false, message = "Booking not found." });

            return Ok(new { success = true, message = "Booking updated successfully." });
        }

        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var session = _httpContextAccessor.HttpContext.Session;
                var userId = session.GetInt32("UserId");

                if (userId == null)
                {
                    return Unauthorized(new { success = false, message = "User is not logged in." });
                }

                var reservations = await _taxiReservationService.GetReservationsAsync(userId.Value);
                return Ok(new { success = true, reservations });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }

        [HttpGet("GetTaxisByTaxiCompany")]
        public async Task<IActionResult> GetTaxisByTaxiCompany(int taxiCompanyId)
        {
            try
            {
                var taxis = await _taxiReservationService.GetTaxisByTaxiCompanyAsync(taxiCompanyId);
                return Ok(taxis);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }

        [HttpPut("UpdateReservation/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] UpdateTaxiReservationViewModel model)
        {
            try
            {
                var updated = await _taxiReservationService.UpdateReservationAsync(reservationId, model);

                if (!updated)
                    return NotFound(new { success = false, message = "Reservation or Taxi not found." });

                return Ok(new { success = true, message = "Reservation updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }

    }
}