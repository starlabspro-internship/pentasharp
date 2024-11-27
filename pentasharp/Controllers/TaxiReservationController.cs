﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.Models.Enums;
using pentasharp.ViewModel.TaxiModels;

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
    }
}