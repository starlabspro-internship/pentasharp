using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.BusSchedul;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Route("api/BusSchedule")]
    [ServiceFilter(typeof(AdminOnlyFilter))]
    public class BusScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BusScheduleController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult BusScheduleManagement() => View();

        [HttpPost("AddRoute")]
        public IActionResult AddRoute([FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.FromLocation) || string.IsNullOrWhiteSpace(model.ToLocation))
                return BadRequest(new { success = false, message = "Invalid data provided." });

            var route = new BusRoutes
            {
                FromLocation = model.FromLocation,
                ToLocation = model.ToLocation,
                EstimatedDuration = new TimeSpan(hours, minutes, 0),
                CreatedAt = DateTime.Now
            };

            _context.BusRoutes.Add(route);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Route added successfully." });
        }

        [HttpGet("GetRoutes")]
        public IActionResult GetRoutes()
        {
            var routes = _context.BusRoutes.ToList();
            var routeViewModels = _mapper.Map<List<AddRouteViewModel>>(routes);
            return Ok(routeViewModels);
        }

        [HttpPut("EditRoute/{id}")]
        public IActionResult EditRoute(int id, [FromBody] AddRouteViewModel model, int hours, int minutes)
        {
            var route = _context.BusRoutes.Find(id);
            if (route == null)
                return NotFound(new { success = false, message = "Route not found." });

            route.FromLocation = model.FromLocation;
            route.ToLocation = model.ToLocation;
            route.EstimatedDuration = new TimeSpan(hours, minutes, 0);

            _context.SaveChanges();
            return Ok(new { success = true, message = "Route updated successfully." });
        }

        [HttpDelete("DeleteRoute/{id}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = _context.BusRoutes.Find(id);
            if (route == null)
                return NotFound(new { success = false, message = "Route not found." });

            _context.BusRoutes.Remove(route);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Route deleted successfully." });
        }

        [HttpPost("AddSchedule")]
        public IActionResult AddSchedule([FromBody] AddScheduleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data provided." });

            var route = _context.BusRoutes.FirstOrDefault(r => r.RouteId == model.RouteId);
            if (route == null)
                return BadRequest(new { success = false, message = "Route not found." });

            var bus = _context.Buses.FirstOrDefault(b => b.BusId == model.BusId);
            if (bus == null)
                return BadRequest(new { success = false, message = "Bus not found." });

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            model.AvailableSeats = bus.Capacity;

            var schedule = _mapper.Map<BusSchedule>(model);
            schedule.Status = BusScheduleStatus.Scheduled;

            _context.BusSchedules.Add(schedule);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Schedule added successfully." });
        }


        [HttpGet("GetSchedules")]
        public IActionResult GetSchedules()
        {
            var schedules = _context.BusSchedules.Include(s => s.Route).Include(s => s.Bus).ToList();
            var scheduleViewModels = schedules.Select(s => new
            {
                s.ScheduleId,
                s.RouteId,
                s.BusId,
                s.DepartureTime,
                s.ArrivalTime,
                s.Price,
                s.AvailableSeats,
                Status = s.Status.ToString()
            }).ToList();

            return Ok(scheduleViewModels);
        }

        [HttpPut("EditSchedule/{id}")]
        public IActionResult EditSchedule(int id, [FromBody] AddScheduleViewModel model)
        {
            var schedule = _context.BusSchedules.Find(id);
            if (schedule == null) return NotFound(new { success = false, message = "Schedule not found." });

            schedule.RouteId = model.RouteId;
            schedule.BusId = model.BusId;
            schedule.DepartureTime = model.DepartureTime;
            schedule.Price = model.Price;
            schedule.AvailableSeats = model.AvailableSeats;

            try
            {
                _context.SaveChanges();
                return Ok(new { success = true, message = "Schedule updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("DeleteSchedule/{id}")]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _context.BusSchedules.Find(id);
            if (schedule == null) return NotFound(new { success = false, message = "Schedule not found." });

            _context.BusSchedules.Remove(schedule);
            _context.SaveChanges();

            return Ok(new { success = true, message = "Schedule deleted successfully." });
        }

        [HttpGet("SearchSchedules")]
        public IActionResult SearchSchedules(string from, string to, DateTime date)
        {
            var schedules = _context.BusSchedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .Where(s => s.Route.FromLocation.Contains(from)
                            && s.Route.ToLocation.Contains(to)
                            && s.DepartureTime.Date == date.Date)
                .Select(s => new
                {
                    s.ScheduleId,
                    s.Route.FromLocation,
                    s.Route.ToLocation,
                    s.DepartureTime,
                    s.ArrivalTime,
                    s.Bus.BusNumber,
                    s.Price,
                    s.AvailableSeats,
                    Status = s.Status.ToString()
                })
                .ToList();

            return Ok(schedules);
        }

        [Route("BusReservationManagement")]
        public IActionResult BusReservationManagement()
        {
            return View();
        }


    }
}