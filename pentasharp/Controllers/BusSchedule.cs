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
    [Route("api/BusSchedule")]
    public class BusScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBusCompanyService _IBusCompanyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusScheduleController(AppDbContext context, IMapper mapper,IBusCompanyService busCompanyService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _IBusCompanyService = busCompanyService;
            _httpContextAccessor = httpContextAccessor;
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        public IActionResult BusScheduleManagement() => View();

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [Route("BusReservationManagement")]
        public IActionResult BusReservationManagement() => View();

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("AddRoute")]
        public async Task<IActionResult> AddRoute([FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            var isInvalidData = !ModelState.IsValid ||
                                string.IsNullOrWhiteSpace(model.FromLocation) ||
                                string.IsNullOrWhiteSpace(model.ToLocation);

            if (isInvalidData)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    ResponseMessages.InvalidData,
                    null 
                ));
            }

            var routeExists = _context.BusRoutes.Any(r =>
                r.FromLocation == model.FromLocation &&
                r.ToLocation == model.ToLocation);

            if (routeExists)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.Conflict,
                    "Route already exists.",
                    null
                ));
            }

            var route = _mapper.Map<BusRoutes>(model);
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);
            route.CreatedAt = DateTime.Now;
            route.BusCompanyId = companyId.Value;

            _context.BusRoutes.Add(route);
            await _context.SaveChangesAsync();

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                ResponseMessages.Success,
                new
                {
                    route.RouteId,
                    route.FromLocation,
                    route.ToLocation,
                    route.EstimatedDuration
                }
            ));
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetRoutes")]
        public async Task<IActionResult> GetRoutes()
        {
           
            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            var routes = await _context.BusRoutes
                .Where(b => b.BusCompanyId == companyId.Value)
                .ToListAsync();

            var routeViewModels = _mapper.Map<List<AddRouteViewModel>>(routes);

            return Ok(routeViewModels);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("EditRoute/{id}")]
        public async Task<IActionResult> EditRoute(int id, [FromBody] AddRouteViewModel model, int hours, int minutes)
        {

            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            var route = _context.BusRoutes.Find(id);
            if (route == null)
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    ResponseMessages.NotFound,
                    null
                ));
            }

            var isDuplicateRoute = _context.BusRoutes
                .Any(r => r.RouteId != id &&
                          r.FromLocation == model.FromLocation &&
                          r.ToLocation == model.ToLocation);

            if (isDuplicateRoute)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    "A route with the same details already exists.",
                    null
                ));
            }

            route.FromLocation = model.FromLocation;
            route.ToLocation = model.ToLocation;
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);

            _context.SaveChanges();

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                ResponseMessages.Success,
                new
                {
                    route.RouteId,
                    route.FromLocation,
                    route.ToLocation,
                    route.EstimatedDuration
                }
            ));
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpDelete("DeleteRoute/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            var route = _context.BusRoutes.SingleOrDefault(r => r.RouteId == id);
            if (route == null)
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    ResponseMessages.NotFound,
                    null
                ));
            }

            _context.BusRoutes.Remove(route);
            _context.SaveChanges();

            return Ok(ResponseFactory.CreateResponse<object>(
                ResponseCodes.Success,
                "Route deleted successfully.",
                null
            ));
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("AddSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] AddScheduleViewModel model)
        {

            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    ResponseMessages.InvalidData,
                    null
                ));
            }

            var route = _context.BusRoutes.SingleOrDefault(r => r.RouteId == model.RouteId);
            if (route == null)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "Route not found.",
                    null
                ));
            }

            var bus = _context.Buses.SingleOrDefault(b => b.BusId == model.BusId);
            if (bus == null)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "Bus not found.",
                    null
                ));
            }

            var duplicateSchedule = _context.BusSchedules
                .SingleOrDefault(s => s.BusId == model.BusId && s.DepartureTime == model.DepartureTime);
            if (duplicateSchedule != null)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.Conflict,
                    "A schedule for the same bus at the same time already exists.",
                    null
                ));
            }

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            model.AvailableSeats = bus.Capacity;
            model.BusCompanyId = companyId.Value;

            var schedule = _mapper.Map<BusSchedule>(model);
            _context.BusSchedules.Add(schedule);
            _context.SaveChanges();

            var responseModel = _mapper.Map<AddScheduleViewModel>(schedule);

            return Ok(ResponseFactory.CreateResponse(
                ResponseCodes.Success,
                "Schedule added successfully.",
                responseModel
            ));
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetSchedules")]
        public async Task<IActionResult> GetSchedules()
        {

            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            var scheduleViewModels = _context.BusSchedules
                .Select(s => new
                {
                    s.ScheduleId,
                    s.RouteId,
                    s.BusId,
                    s.DepartureTime,
                    s.ArrivalTime,
                    s.Price,
                    s.AvailableSeats,
                    s.BusCompanyId,
                    Status = s.Status.ToString()
                })
                .Where(s => s.BusCompanyId == companyId.Value)
                .ToList();

            return Ok(scheduleViewModels);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("EditSchedule/{id}")]
        public async Task<IActionResult> EditSchedule(int id, [FromBody] AddScheduleViewModel model)
        {

            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            if (!userExists)
            {
                return Unauthorized(new { success = false, message = "Invalid business type or user not logged in." });
            }

            var schedule = _context.BusSchedules.Find(id);
            if (schedule == null)
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "Schedule not found.",
                    null
                ));
            }

            var isDuplicateSchedule = _context.BusSchedules
                .Any(s => s.ScheduleId != id &&
                          s.BusId == model.BusId &&
                          s.DepartureTime == model.DepartureTime);

            if (isDuplicateSchedule)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    "A schedule for the same bus at the same time already exists.",
                    null
                ));
            }

            schedule.RouteId = model.RouteId;
            schedule.BusId = model.BusId;
            schedule.DepartureTime = model.DepartureTime;
            schedule.Price = model.Price;
            schedule.AvailableSeats = model.AvailableSeats;

            try
            {
                _context.SaveChanges();

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    "Schedule updated successfully.",
                    new
                    {
                        schedule.ScheduleId,
                        schedule.RouteId,
                        schedule.BusId,
                        schedule.DepartureTime,
                        schedule.ArrivalTime,
                        schedule.Price,
                        schedule.AvailableSeats
                    }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InternalServerError,
                    ex.Message,
                    null
                ));
            }
        }
    }
}