using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiModels;
using pentasharp.ViewModel.TaxiReservation;
using WebApplication1.Filters;

namespace pentasharp.Controllers
{
    [Route("api/TaxiReservation")]
    public class TaxiReservationController : Controller
    {
        private readonly AppDbContext _context;

        public TaxiReservationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("SearchAvailableTaxis")]
        public IActionResult SearchAvailableTaxis()
        {
            var taxiCompanies = _context.TaxiCompanies
                .Select(c => new
                {
                    c.TaxiCompanyId,
                    c.CompanyName,
                    c.ContactInfo
                })
                .ToList();

            return Ok(new { success = true, companies = taxiCompanies });
        }
        [ServiceFilter(typeof(LoginRequiredFilter))]
        [HttpPost("CreateReservation")]
        public IActionResult CreateReservation([FromBody] TaxiReservationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new { success = false, message = "Invalid data provided.", errors });
                }

                if (!TimeSpan.TryParse(model.ReservationTime, out var timeSpan))
                {
                    return BadRequest(new { success = false, message = "Invalid ReservationTime format. Must be HH:mm:ss." });
                }

                var reservation = new TaxiReservations
                {
                    TaxiCompanyId = model.TaxiCompanyId,
                    UserId = model.UserId,
                    PickupLocation = model.PickupLocation,
                    DropoffLocation = model.DropoffLocation,
                    ReservationTime = model.ReservationDate.Add(timeSpan),
                    PassengerCount = model.PassengerCount,
                    Fare = 0,
                    Status = ReservationStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TaxiReservations.Add(reservation);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Reservation created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }
        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetReservations")]
        public IActionResult GetReservations()
        {
            try
            {
                var reservations = _context.TaxiReservations
                .Include(r => r.User)
                .Include(r => r.Taxi)
                .Include(r => r.TaxiCompany)
                .Select(r => new
                {
                    TaxiCompanyId = r.TaxiCompanyId,
                    ReservationId = r.ReservationId,
                    PassengerName = r.User.FirstName,
                    PickupLocation = r.PickupLocation,
                    DropoffLocation = r.DropoffLocation,
                    ReservationDate = r.ReservationTime.ToString("yyyy-MM-dd"),
                    ReservationTime = r.ReservationTime.ToString("hh:mm tt"),
                    Status = r.Status.ToString(),
                    Driver = r.Taxi != null ? $"{r.Taxi.DriverName} - {r.Taxi.LicensePlate}" : "Unassigned"
                })
                .ToList();

                return Ok(new { success = true, reservations });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }
        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpGet("GetTaxisByTaxiCompany")]
        public IActionResult GetTaxisByTaxiCompany(int taxiCompanyId)
        {
            try
            {
                var taxis = _context.Taxis
                    .Where(t => t.TaxiCompanyId == taxiCompanyId)
                    .Select(t => new
                    {
                        TaxiId = t.TaxiId,
                        DriverName = t.DriverName,
                        LicensePlate = t.LicensePlate
                    })
                    .ToList();

                return Ok(taxis);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }
        [ServiceFilter(typeof(AdminOnlyFilter))]
        [HttpPut("UpdateReservation/{reservationId}")]
        public IActionResult UpdateReservation(int reservationId, [FromBody] UpdateReservationViewModel model)
        {
            try
            {
                var reservation = _context.TaxiReservations
                    .Include(r => r.Taxi)
                    .FirstOrDefault(r => r.ReservationId == reservationId);

                if (reservation == null)
                {
                    return NotFound(new { success = false, message = "Reservation not found." });
                }

                var taxi = _context.Taxis.FirstOrDefault(t => t.TaxiId == model.TaxiId);
                if (taxi == null && model.TaxiId != 0)
                {
                    return NotFound(new { success = false, message = "Taxi not found." });
                }
                if (model.ReservationDate.HasValue && !string.IsNullOrEmpty(model.ReservationTime))
                {
                    var reservationDateTime = model.ReservationDate.Value;
                    reservation.ReservationTime = reservationDateTime;
                    Console.WriteLine("Date: " + model.ReservationDate.Value);
                    Console.WriteLine("Time: " + model.ReservationTime);
                }
                else
                {
                    return BadRequest(new { success = false, message = "ReservationDate or ReservationTime is missing." });
                }

                reservation.TaxiId = model.TaxiId;
                reservation.Fare = model.Fare;
                reservation.PickupLocation = model.PickupLocation;
                reservation.DropoffLocation = model.DropoffLocation;
                reservation.Status = Enum.Parse<ReservationStatus>(model.Status);
                reservation.UpdatedAt = DateTime.UtcNow;

                _context.TaxiReservations.Update(reservation);
                _context.SaveChanges();

                return Ok(new { success = true, message = "Reservation updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred.", error = ex.Message });
            }
        }
    }
}