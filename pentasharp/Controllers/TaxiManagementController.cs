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
            var bookings = await _taxiBookingService.GetAllBookingsAsync();

            return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, bookings));
        }

        [HttpGet("GetBooking")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _taxiBookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, ResponseMessages.NotFound));
            }

            return Ok(ResponseFactory.SuccessResponse(ResponseMessages.Success, booking));
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] EditTaxiBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));

            var success = await _taxiBookingService.UpdateBookingAsync(model);
            if (!success)
                return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Booking not found."));

            return Ok(ResponseFactory.SuccessResponse("Booking updated successfully.",success));
        }

        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservations = await _taxiReservationService.GetReservationsAsync();
                return Ok(new { success = true, reservations });
            }
            catch (Exception ex)
            {
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
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, ResponseMessages.InternalServerError));
            }
        }

        [HttpPut("UpdateReservation/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] UpdateTaxiReservationViewModel model)
        {
            try
            {
                var updated = await _taxiReservationService.UpdateReservationAsync(reservationId, model);

                if (!updated)
                    return NotFound(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "Reservation or Taxi not found."));

                return Ok(ResponseFactory.SuccessResponse("Reservation updated successfully.",updated));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, ResponseMessages.InternalServerError));
            }
        }
    }
}