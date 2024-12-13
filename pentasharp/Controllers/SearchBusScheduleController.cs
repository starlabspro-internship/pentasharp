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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchBusScheduleController(AppDbContext context, IMapper mapper, IBusCompanyService busCompanyService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _IBusCompanyService = busCompanyService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpGet("SearchSchedules")]
        public IActionResult SearchSchedules(string from, string to, DateTime date)
        {

            var schedules = _context.BusSchedules
                .Where(s => s.Route.FromLocation.Contains(from)
                            && s.Route.ToLocation.Contains(to)
                            && s.DepartureTime.Date == date.Date)
                .ProjectTo<SearchScheduleViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            if (!schedules.Any())
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "No schedules found matching the search criteria.",
                    null
                ));
            }

            return Ok(schedules);
        }

        [AllowAnonymous]
        [HttpGet("GetFromLocationSuggestions")]
        public IActionResult GetFromLocationSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.InvalidData,
                    ResponseMessages.InvalidData,
                    null
                ));
            }

            var suggestions = _context.BusRoutes
                .Where(r => r.FromLocation.StartsWith(query))
                .Select(r => r.FromLocation)
                .Distinct()
                .Take(10)
                .ToList();

            if (!suggestions.Any())
            {
                return NotFound(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.NotFound,
                    ResponseMessages.NotFound,
                    suggestions.ToArray()
                ));
            }

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                ResponseMessages.Success,
                suggestions.ToArray()
            ));
        }

        [AllowAnonymous]
        [HttpGet("GetToLocationSuggestions")]
        public IActionResult GetToLocationSuggestions(string fromLocation, string query)
        {
            if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(fromLocation))
            {
                return BadRequest(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.InvalidData,
                    ResponseMessages.InvalidData,
                    null
                ));
            }

            var suggestions = _context.BusRoutes
                .Where(r => r.FromLocation == fromLocation && r.ToLocation.StartsWith(query))
                .Select(r => r.ToLocation)
                .Distinct()
                .Take(10)
                .ToList();

            if (!suggestions.Any())
            {
                return NotFound(ResponseFactory.CreateResponse<string[]>(
                    ResponseCodes.NotFound,
                    ResponseMessages.NotFound,
                    suggestions.ToArray()
                ));
            }

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                ResponseMessages.Success,
                suggestions.ToArray()
            ));
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [Route("BusReservationManagement")]
        public IActionResult BusReservationManagement()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("AddReservation")]
        public IActionResult AddReservation([FromBody] AddBusReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    "Invalid data.",
                    null
                ));
            }

            try
            {
                var schedule = _context.BusSchedules
                    .Where(s => s.ScheduleId == model.ScheduleId)
                    .Select(s => new
                    {
                        s.ScheduleId,
                        s.BusCompanyId,
                        s.AvailableSeats,
                        s.Status
                    })
                    .FirstOrDefault();

                if (schedule == null)
                {
                    return NotFound(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.NotFound,
                        "The schedule does not exist.",
                        null
                    ));
                }

                if (schedule.Status != BusScheduleStatus.Scheduled)
                {
                    return BadRequest(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.InvalidData,
                        "The schedule is not active for reservations.",
                        null
                    ));
                }

                if (schedule.AvailableSeats < model.NumberOfSeats)
                {
                    return BadRequest(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.InvalidData,
                        "Not enough seats available for the selected schedule.",
                        null
                    ));
                }

                var reservation = _mapper.Map<BusReservations>(model);
                _context.BusReservations.Add(reservation);
                _context.SaveChanges();

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    "Reservation added successfully.",
                    new { reservation.ReservationId }
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