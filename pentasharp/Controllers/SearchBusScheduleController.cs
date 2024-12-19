using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.BusSchedul;
using pentasharp.ViewModel.BusReservation;
using WebApplication1.Filters;
using Microsoft.AspNetCore.Authorization;
using AutoMapper.QueryableExtensions;
using pentasharp.Models.Utilities;
using pentasharp.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.Controllers
{
    [Route("api/SearchSchedule")]
    public class SearchBusScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBusCompanyService _IBusCompanyService;
        private readonly ISearchBusScheduleService _scheduleService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchBusScheduleController(AppDbContext context, IMapper mapper, IBusCompanyService busCompanyService, IHttpContextAccessor httpContextAccessor, ISearchBusScheduleService scheduleService)
        {
            _context = context;
            _mapper = mapper;
            _IBusCompanyService = busCompanyService;
            _httpContextAccessor = httpContextAccessor;
            _scheduleService = scheduleService;
        }

        [AllowAnonymous]
        [HttpGet("SearchSchedules")]
        public async Task<IActionResult> SearchSchedules(string from, string to, DateTime date)
        {
            var schedules = await _scheduleService.SearchSchedulesAsync(from, to, date);

            if (!schedules.Any())
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "No schedules found matching the search criteria.",
                    null
                ));
            }

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                "Schedules retrieved successfully.",
                schedules
            ));
        }

        [AllowAnonymous]
        [HttpGet("GetFromLocationSuggestions")]
        public async Task<IActionResult> GetFromLocationSuggestions(string query)
        {
            try
            {
                var suggestions = await _scheduleService.GetFromLocationSuggestionsAsync(query);

                if (!suggestions.Any())
                {
                    return NotFound(ResponseFactory.CreateResponse(
                        ResponseCodes.NotFound,
                        ResponseMessages.NotFound,
                        suggestions
                    ));
                }

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    ResponseMessages.Success,
                    suggestions
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.InvalidData,
                    ex.Message,
                    null
                ));
            }
        }

        [AllowAnonymous]
        [HttpGet("GetToLocationSuggestions")]
        public async Task<IActionResult> GetToLocationSuggestions(string fromLocation, string query)
        {
            try
            {
                var suggestions = await _scheduleService.GetToLocationSuggestionsAsync(fromLocation, query);

                if (!suggestions.Any())
                {
                    return NotFound(ResponseFactory.CreateResponse(
                        ResponseCodes.NotFound,
                        ResponseMessages.NotFound,
                        suggestions
                    ));
                }

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    ResponseMessages.Success,
                    suggestions
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.InvalidData,
                    ex.Message,
                    null
                ));
            }
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("AddReservation")]
        public async Task<IActionResult> AddReservation([FromBody] AddBusReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    ResponseMessages.InvalidData,
                    null
                ));
            }

            try
            {
                var reservationId = await _scheduleService.AddReservationAsync(model);

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    "Reservation added successfully.",
                    new { reservationId }
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    ex.Message,
                    null
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    ex.Message,
                    null
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InternalServerError,
                    "An error occurred while adding the reservation.",
                    new { details = ex.Message }
                ));
            }
        }
    }
}