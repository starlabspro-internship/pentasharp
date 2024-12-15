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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaxiBookingController(ITaxiBookingService taxiBookingService, IHttpContextAccessor httpContextAccessor)
        {
            _taxiBookingService = taxiBookingService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("SearchAvailableTaxis")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _taxiBookingService.GetAllCompaniesAsync();
                if (companies == null || !companies.Any())
                {
                    return Ok(ResponseFactory.ErrorResponse(ResponseCodes.NotFound, "No companies found."));
                }

                return Ok(new { success = true, companies });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, ResponseFactory.ErrorResponse(ResponseCodes.InternalServerError, ResponseMessages.InternalServerError));
            }
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, ResponseMessages.InvalidData));

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
                return BadRequest(ResponseFactory.ErrorResponse(ResponseCodes.InvalidData, "Booking creation failed."));

            return Ok(ResponseFactory.SuccessResponse("Booking created successfully",success));
        }
    }
}