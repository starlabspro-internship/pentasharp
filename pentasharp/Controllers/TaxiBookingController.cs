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
using pentasharp.ViewModel.TaxiModels;
using WebApplication1.Filters;
using pentasharp.Interfaces;
using pentasharp.Models.TaxiRequest;

namespace WebApplication1.Controllers
{
    [Route("api/TaxiBooking")]
    public class TaxiBookingController : Controller
    {
        private readonly ITaxiBookingService _taxiBookingService;

        public TaxiBookingController(ITaxiBookingService taxiBookingService)
        {
            _taxiBookingService = taxiBookingService;
        }

        [AllowAnonymous]
        [HttpPost("SearchAvailableTaxis")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _taxiBookingService.GetAllCompaniesAsync();
            return Ok(new { success = true, companies });
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            var request = new TaxiBookingRequest
            {
                TaxiCompanyId = model.TaxiCompanyId,
                PickupLocation = model.PickupLocation,
                DropoffLocation = model.DropoffLocation,
                BookingTime = model.BookingTime,
                PassengerCount = model.PassengerCount,
                UserId = model.UserId,
                Status = ReservationStatus.Pending 
            };

            var success = await _taxiBookingService.CreateBookingAsync(request);
            if (!success)
                return BadRequest(new { success = false, message = "Booking creation failed." });

            return Ok(new { success = true, message = "Booking created successfully" });
        }


        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _taxiBookingService.GetAllBookingsAsync();
            return Ok(new StandardApiResponse<List<TaxiBookingViewModel>>
            {
                Success = true,
                Message = ResponseMessages.Success,
                Code = ResponseCodes.Success,
                Data = bookings
            });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
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

        [ServiceFilter(typeof(AdminOnlyFilter))]
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
    }
}