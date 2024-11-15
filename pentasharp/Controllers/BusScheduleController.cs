using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pentasharp.Data;
using WebApplication1.Filters;
using pentasharp.ViewModel.BusSchedule;
using pentasharp.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("BusScheduleManagement")]
        public IActionResult BusScheduleManagement()
        {
            return View();
        }

        [HttpGet("ConfirmPassengers")]
        public IActionResult ConfirmPassengers()
        {
            return View();
        }

        [HttpGet("ManageBusSchedules")]
        public IActionResult ManageBusSchedules()
        {
            return View();
        }

        [HttpPost("AddRoute")]
        public IActionResult AddRoute(AddRouteViewModel routeModel, int hours, int minutes)
        {
            if (ModelState.IsValid)
            {
                routeModel.EstimatedDuration = new TimeSpan(hours, minutes, 0);
                var newRoute = _mapper.Map<BusRoutes>(routeModel);
                newRoute.CreatedAt = DateTime.Now;

                _context.BusRoutes.Add(newRoute);
                _context.SaveChanges();

                return RedirectToAction("ManageBusSchedules");
            }
            return View(routeModel);
        }

        [HttpGet("GetRoutes")]
        public IActionResult GetRoutes()
        {
            var routes = _context.BusRoutes.ToList();
            var routeViewModels = _mapper.Map<List<AddRouteViewModel>>(routes);

            return Ok(routeViewModels);
        }

        [HttpGet("GetRouteById/{id}")]
        public IActionResult GetRouteById(int id)
        {
            var route = _context.BusRoutes.Find(id);
            if (route == null)
            {
                return NotFound();
            }

            var routeViewModel = _mapper.Map<AddRouteViewModel>(route);
            return Ok(routeViewModel);
        }

        [HttpPost("EditRoute/{id}")]
        public IActionResult EditRoute(int id, AddRouteViewModel routeModel, int hours, int minutes)
        {
            if (ModelState.IsValid)
            {
                var existingRoute = _context.BusRoutes.Find(id);
                if (existingRoute == null)
                {
                    return NotFound();
                }

                existingRoute.FromLocation = routeModel.FromLocation;
                existingRoute.ToLocation = routeModel.ToLocation;
                existingRoute.EstimatedDuration = new TimeSpan(hours, minutes, 0);
                existingRoute.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                return RedirectToAction("ManageBusSchedules");
            }
            return View(routeModel);
        }

        [HttpDelete("DeleteRoute/{id}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = _context.BusRoutes.Find(id);
            if (route == null)
            {
                return NotFound(new { Message = "Route not found" });
            }

            _context.BusRoutes.Remove(route);
            _context.SaveChanges();

            return Ok(new { Message = "Route deleted successfully" });
        }

        [HttpGet("GetAllRoutes")]
        public IActionResult GetAllRoutes()
        {
            var routes = _context.BusRoutes
                .Select(r => new { r.RouteId, RouteName = $"{r.FromLocation} - {r.ToLocation}" })
                .ToList();

            return Ok(routes);
        }

        [HttpGet("GetRouteBy/{id}")]
        public IActionResult GetRouteBy(int id)
        {
            var route = _context.BusRoutes.FirstOrDefault(r => r.RouteId == id);
            if (route == null)
            {
                return NotFound(new { Message = "Route not found" });
            }

            return Ok(new
            {
                RouteId = route.RouteId,
                EstimatedDuration = route.EstimatedDuration.ToString(@"hh\:mm\:ss")
            });
        }

        [HttpPost("AddSchedule")]
        public IActionResult AddSchedule([FromBody] AddScheduleViewModel model)
        {
            if (model == null)
                return BadRequest(new { Message = "Invalid schedule data" });

            var route = _context.BusRoutes.FirstOrDefault(r => r.RouteId == model.RouteId);
            if (route == null)
                return BadRequest(new { Message = "Route not found" });

            model.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);

            var busSchedule = _mapper.Map<BusSchedule>(model);
            _context.BusSchedules.Add(busSchedule);
            _context.SaveChanges();

            return Ok(new { Message = "Schedule added successfully", RedirectUrl = Url.Action("ManageBusSchedules") });
        }

        [HttpPost("EditSchedule/{id}")]
        public IActionResult EditSchedule(int id, [FromBody] EditScheduleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid input data" });

            var existingSchedule = _context.BusSchedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (existingSchedule == null)
                return NotFound(new { Message = "Schedule not found" });

            var route = _context.BusRoutes.FirstOrDefault(r => r.RouteId == model.RouteId);
            if (route == null)
                return BadRequest(new { Message = "Route not found" });

            existingSchedule.RouteId = model.RouteId;
            existingSchedule.BusId = model.BusId;
            existingSchedule.DepartureTime = model.DepartureTime;
            existingSchedule.ArrivalTime = model.DepartureTime.Add(route.EstimatedDuration);
            existingSchedule.Price = model.Price;
            existingSchedule.AvailableSeats = model.AvailableSeats;

            _context.SaveChanges();

            return Ok(new { Message = "Schedule updated successfully", RedirectUrl = Url.Action("ManageBusSchedules") });
        }

        [HttpGet("GetAllSchedules")]
        public IActionResult GetAllSchedules()
        {
            var schedules = _context.BusSchedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .ThenInclude(b => b.BusCompany)
                .Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    RouteName = $"{s.Route.FromLocation} - {s.Route.ToLocation}",
                    BusNumber = s.Bus.BusNumber,
                    CompanyName = s.Bus.BusCompany.CompanyName,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    Price = s.Price,
                    Status = s.Status.ToString(),
                    AvailableSeats = s.AvailableSeats
                })
                .ToList();

            return Ok(schedules);
        }

        [HttpGet("GetScheduleById/{id}")]
        public IActionResult GetScheduleById(int id)
        {
            var schedule = _context.BusSchedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null) return NotFound();

            return Ok(new
            {
                schedule.ScheduleId,
                schedule.RouteId,
                schedule.BusId,
                schedule.DepartureTime,
                schedule.ArrivalTime,
                schedule.Price,
                schedule.AvailableSeats
            });
        }

        [HttpDelete("DeleteSchedule/{id}")]
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _context.BusSchedules.Find(id);
            if (schedule == null)
            {
                return NotFound(new { Message = "Schedule not found" });
            }

            _context.BusSchedules.Remove(schedule);
            _context.SaveChanges();

            return Ok(new { Message = "Schedule deleted successfully" });
        }
    }
}