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
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid data provided."
                });
            }

            var routeExists = _context.BusRoutes.Any(r =>
                r.FromLocation == model.FromLocation &&
                r.ToLocation == model.ToLocation);

            if (routeExists)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Route already exists."
                });
            }

            var route = _mapper.Map<BusRoutes>(model);
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);
            route.CreatedAt = DateTime.Now;

            _context.BusRoutes.Add(route);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Route added successfully."
            });
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
            var route = _context.BusRoutes.SingleOrDefault(r => r.RouteId == id);
            if (route == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Route not found."
                });
            }

            var isDuplicateRoute = _context.BusRoutes
                .Any(r => r.RouteId != id &&
                          r.FromLocation == model.FromLocation &&
                          r.ToLocation == model.ToLocation);

            if (isDuplicateRoute)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "A route with the same details already exists."
                });
            }

            route.FromLocation = model.FromLocation;
            route.ToLocation = model.ToLocation;
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);

            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Route updated successfully."
            });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpDelete("DeleteRoute/{id}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = _context.BusRoutes.SingleOrDefault(r => r.RouteId == id);
              if (route == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Route not found."
                });
            }

            _context.BusRoutes.Remove(route);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Route deleted successfully." });
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("AddSchedule")]
        public IActionResult AddSchedule([FromBody] AddScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid data provided.",
                    data = (object)null
                });
            }

            var route = _context.BusRoutes.SingleOrDefault(r => r.RouteId == model.RouteId);
            if (route == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Route not found.",
                    data = (object)null
                });
            }

            var bus = _context.Buses.SingleOrDefault(b => b.BusId == model.BusId);
            if (bus == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Bus not found.",
                    data = (object)null
                });
            }

            var duplicateSchedule = _context.BusSchedules
                .SingleOrDefault(s => s.BusId == model.BusId && s.DepartureTime == model.DepartureTime);
            if (duplicateSchedule != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "A schedule for the same bus at the same time already exists.",
                    data = (object)null
                });
            }

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            model.AvailableSeats = bus.Capacity;

            var schedule = _mapper.Map<BusSchedule>(model);

            _context.BusSchedules.Add(schedule);
            _context.SaveChanges();

            var responseModel = _mapper.Map<AddScheduleViewModel>(schedule);

            return Ok(new
            {
                success = true,
                message = "Schedule added successfully.",
                data = responseModel
            });
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
            var schedule = _context.BusSchedules.SingleOrDefault(s => s.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Schedule not found."
                });
            }

            var isDuplicateSchedule = _context.BusSchedules
                .Any(s => s.ScheduleId != id &&
                          s.BusId == model.BusId &&
                          s.DepartureTime == model.DepartureTime);
            if (isDuplicateSchedule)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "A schedule for the same bus at the same time already exists."
                });
            }

            schedule.RouteId = model.RouteId;
            schedule.BusId = model.BusId;
            schedule.DepartureTime = model.DepartureTime;
            schedule.Price = model.Price;
            schedule.AvailableSeats = model.AvailableSeats;

            try
            {
                _context.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "Schedule updated successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpDelete("DeleteSchedule/{id}")]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _context.BusSchedules.SingleOrDefault(s => s.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Schedule not found."
                });
            }

            _context.BusSchedules.Remove(schedule);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Schedule deleted successfully." });
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

            return Ok(schedules);
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
                return BadRequest(new { success = false, message = "Invalid data." });
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
                    return BadRequest(new { success = false, message = "The schedule does not exist." });
                }

                if (schedule.Status != BusScheduleStatus.Scheduled)
                {
                    return BadRequest(new { success = false, message = "The schedule is not active for reservations." });
                }

                if (schedule.AvailableSeats < model.NumberOfSeats)
                {
                    return BadRequest(new { success = false, message = "Not enough seats available for the selected schedule." });
                }

                var reservation = _mapper.Map<BusReservations>(model);

                _context.BusReservations.Add(reservation);

                _context.SaveChanges();

                return Ok(new { success = true, message = "Reservation added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while adding the reservation.",
                    details = ex.Message
                });
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
                    r.ReservationId,
                    r.ReservationDate,
                    r.NumberOfSeats,
                    r.TotalAmount,
                    r.PaymentStatus,
                    r.Status,
                    Schedule = new
                    {
                        r.Schedule.ScheduleId,
                        r.Schedule.DepartureTime,
                        r.Schedule.ArrivalTime,
                        r.Schedule.Price,
                        r.Schedule.AvailableSeats,
                        BusNumber = r.Schedule.Bus.BusNumber,
                        FromLocation = r.Schedule.Route.FromLocation,
                        ToLocation = r.Schedule.Route.ToLocation
                    }
                })
                .ToList();

            return Ok(reservations);
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("ConfirmReservation")]
        public IActionResult ConfirmReservation([FromBody] EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid request. Reservation ID is required." });
            }

            try
            {
                var reservation = _context.BusReservations
                    .Where(r => r.ReservationId == model.ReservationId)
                    .Select(r => r)
                    .FirstOrDefault();

                if (reservation == null)
                {
                    return NotFound(new { success = false, message = "Reservation not found." });
                }

                if (reservation.Status == BusReservationStatus.Confirmed)
                {
                    return BadRequest(new { success = false, message = "Reservation is already confirmed." });
                }

                var scheduleToUpdate = _context.BusSchedules.SingleOrDefault(s => s.ScheduleId == reservation.ScheduleId);
                if (scheduleToUpdate != null)
                {
                    if (scheduleToUpdate.AvailableSeats < reservation.NumberOfSeats)
                    {
                        return BadRequest(new { success = false, message = "Not enough available seats to confirm this reservation." });
                    }

                    scheduleToUpdate.AvailableSeats -= reservation.NumberOfSeats;
                }

                reservation.Status = BusReservationStatus.Confirmed;
                reservation.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return Ok(new { success = true, message = "Reservation confirmed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while confirming the reservation.",
                    details = ex.Message
                });
            }
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPost("CancelReservation")]
        public IActionResult CancelReservation([FromBody] EditReservationViewModel model)
        {
            if (model == null || model.ReservationId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid request. Reservation ID is required." });
            }

            try
            {
                var reservation = _context.BusReservations
                    .Where(r => r.ReservationId == model.ReservationId)
                    .Select(r => r)
                    .FirstOrDefault();

                if (reservation == null)
                {
                    return NotFound(new { success = false, message = "Reservation not found." });
                }

                var scheduleToUpdate = _context.BusSchedules.SingleOrDefault(s => s.ScheduleId == reservation.ScheduleId);
                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.AvailableSeats += reservation.NumberOfSeats;
                }

                reservation.Status = BusReservationStatus.Canceled;
                reservation.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();

                return Ok(new { success = true, message = "Reservation canceled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while canceling the reservation.",
                    details = ex.Message
                });
            }
        }
    }
}