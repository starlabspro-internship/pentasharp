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

namespace WebApplication1.Controllers
{
    [Route("api/BusSchedule")]
    public class BusScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BusScheduleController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        public IActionResult BusScheduleManagement() => View();

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("AddRoute")]
        public IActionResult AddRoute([FromBody] AddRouteViewModel model, int hours, int minutes)
        {
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

            _context.BusRoutes.Add(route);
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
        [HttpGet("GetRoutes")]
        public IActionResult GetRoutes()
        {
            var routes = _context.BusRoutes.ToList();
            var routeViewModels = _mapper.Map<List<AddRouteViewModel>>(routes);
            return Ok(routeViewModels);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("EditRoute/{id}")]
        public IActionResult EditRoute(int id, [FromBody] AddRouteViewModel model, int hours, int minutes)
        {

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
        public IActionResult DeleteRoute(int id)
        {
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
        public IActionResult AddSchedule([FromBody] AddScheduleViewModel model)
        {

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
        public IActionResult GetSchedules()
        {
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
                    Status = s.Status.ToString()
                })
                .ToList();

            return Ok(scheduleViewModels);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("EditSchedule/{id}")]
        public IActionResult EditSchedule(int id, [FromBody] AddScheduleViewModel model)
        {

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

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpDelete("DeleteSchedule/{id}")]
        public IActionResult DeleteSchedule(int id)
        {

            var schedule = _context.BusSchedules.Find(id);
            if (schedule == null)
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "Schedule not found.",
                    null
                ));
            }

            _context.BusSchedules.Remove(schedule);
            _context.SaveChanges();

            return Ok(ResponseFactory.CreateResponse<object>(
                ResponseCodes.Success,
                "Schedule deleted successfully.",
                null
            ));
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

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetReservations")]
        public IActionResult GetReservations()
        {
            var reservations = _context.BusReservations
                .Where(r => r.Schedule != null && r.Schedule.Route != null && r.Schedule.Bus != null)
                .Select(r => new
                {
                    ReservationId = r.ReservationId,
                    ReservationDate = r.ReservationDate,
                    NumberOfSeats = r.NumberOfSeats,
                    TotalAmount = r.TotalAmount,
                    PaymentStatus = r.PaymentStatus,
                    Status = r.Status,
                    Schedule = new
                    {
                        ScheduleId = r.Schedule.ScheduleId,
                        DepartureTime = r.Schedule.DepartureTime,
                        ArrivalTime = r.Schedule.ArrivalTime,
                        Price = r.Schedule.Price,
                        AvailableSeats = r.Schedule.AvailableSeats,
                        BusNumber = r.Schedule.Bus.BusNumber,
                        FromLocation = r.Schedule.Route.FromLocation,
                        ToLocation = r.Schedule.Route.ToLocation
                    }
                })
                .ToList();

            if (!reservations.Any())
            {
                return NotFound(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.NotFound,
                    "No reservations found.",
                    null
                ));
            }

            return Ok(reservations);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("ConfirmReservation")]
        public IActionResult ConfirmReservation([FromBody] EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    "Invalid request. Reservation ID is required.",
                    null
                ));
            }

            try
            {
                var reservation = _context.BusReservations
                    .Where(r => r.ReservationId == model.ReservationId)
                    .FirstOrDefault();

                if (reservation == null)
                {
                    return NotFound(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.NotFound,
                        "Reservation not found.",
                        null
                    ));
                }

                if (reservation.Status == BusReservationStatus.Confirmed)
                {
                    return BadRequest(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.InvalidData,
                        "Reservation is already confirmed.",
                        null
                    ));
                }

                var scheduleToUpdate = _context.BusSchedules.Find(reservation.ScheduleId);
                if (scheduleToUpdate != null)
                {
                    if (scheduleToUpdate.AvailableSeats < reservation.NumberOfSeats)
                    {
                        return BadRequest(ResponseFactory.CreateResponse<object>(
                            ResponseCodes.InvalidData,
                            "Not enough available seats to confirm this reservation.",
                            null
                        ));
                    }

                    scheduleToUpdate.AvailableSeats -= reservation.NumberOfSeats;
                }

                reservation.Status = BusReservationStatus.Confirmed;
                reservation.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    "Reservation confirmed successfully.",
                    new
                    {
                        reservation.ReservationId,
                        reservation.Status,
                        reservation.UpdatedAt
                    }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InternalServerError,
                    "An error occurred while confirming the reservation.",
                    new { details = ex.Message }
                ));
            }
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("CancelReservation")]
        public IActionResult CancelReservation([FromBody] EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
            {
                return BadRequest(ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InvalidData,
                    "Invalid request. Reservation ID is required.",
                    null
                ));
            }

            try
            {
                var reservation = _context.BusReservations
                    .Where(r => r.ReservationId == model.ReservationId)
                    .FirstOrDefault();

                if (reservation == null)
                {
                    return NotFound(ResponseFactory.CreateResponse<object>(
                        ResponseCodes.NotFound,
                        "Reservation not found.",
                        null
                    ));
                }

                var scheduleToUpdate = _context.BusSchedules.Find(reservation.ScheduleId);
                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.AvailableSeats += reservation.NumberOfSeats;
                }

                reservation.Status = BusReservationStatus.Canceled;
                reservation.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return Ok(ResponseFactory.CreateResponse(
                    ResponseCodes.Success,
                    "Reservation canceled successfully.",
                    new
                    {
                        reservation.ReservationId,
                        reservation.Status,
                        reservation.UpdatedAt
                    }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseFactory.CreateResponse<object>(
                    ResponseCodes.InternalServerError,
                    "An error occurred while canceling the reservation.",
                    new { details = ex.Message }
                ));
            }
        }
    }
}