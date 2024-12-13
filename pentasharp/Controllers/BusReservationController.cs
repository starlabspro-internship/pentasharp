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
    [Route("api/BusReservation")]
    public class BusReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBusCompanyService _IBusCompanyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusReservationController(AppDbContext context, IMapper mapper, IBusCompanyService busCompanyService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _IBusCompanyService = busCompanyService;
            _httpContextAccessor = httpContextAccessor;
        }

        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetReservations")]
        public async Task<IActionResult> GetReservations()
        {
            var companyId = _httpContextAccessor.HttpContext.Session.GetInt32("CompanyId");

            if (!companyId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Company not logged in." });
            }

            var userExists = await _context.Users.AnyAsync(u =>
                u.CompanyId == companyId.Value &&
                u.BusinessType == BusinessType.BusCompany);

            var reservations = _context.BusReservations
                .Where(r => r.Schedule != null && r.Schedule.Route != null && r.Schedule.Bus != null && r.BusCompanyId == companyId.Value)
                .Select(r => new
                {
                    ReservationId = r.ReservationId,
                    ReservationDate = r.ReservationDate,
                    NumberOfSeats = r.NumberOfSeats,
                    TotalAmount = r.TotalAmount,
                    PaymentStatus = r.PaymentStatus,
                    Status = r.Status,
                    User = new
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName
                    },
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