using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pentasharp.Data;
using pentasharp.Models.Entities;
using pentasharp.ViewModel.TaxiReservation;
using System;
using System.Linq;
using WebApplication1.Filters;
using pentasharp.Models.Enums;
using Microsoft.AspNetCore.Http; // For session handling
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers

{
    [Route("IncomingTaxiReservation")]
    [ServiceFilter(typeof(LoginRequiredFilter))]
    public class IncomingTaxiReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<IncomingTaxiReservationController> _logger;

        public IncomingTaxiReservationController(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<IncomingTaxiReservationController> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // POST: Search for available 
        [HttpPost("Search")]
        public IActionResult Search([FromBody] TaxiSearchViewModel searchModel)
        {
            var availableTaxis = _context.TaxiCompanies
                                        .Where(t => t.Taxis.Any()) // Ensuring the company has taxis
                                        .ToList();

            var taxiResults = availableTaxis.Select(t => new
            {
                CompanyName = t.CompanyName,
                Taxi = t.Taxis.FirstOrDefault()?.LicensePlate ?? "No available taxi", // Adjust based on your `Taxi` class
                ContactInfo = t.ContactInfo
            }).ToList();

            _logger.LogInformation("Taxi Company Results: {@TaxiResults}", taxiResults);
            return Json(taxiResults);
        }
        // POST: Reserve Taxi
        // POST: Reserve Taxi

        //[HttpPost("TaxiReservations")]


        //public IActionResult TaxiReservations([FromBody] TaxiReservationViewModel model)
        //{
        //    try
        //    {
        //        // Log the incoming reservation model
        //        _logger.LogInformation("Received reservation model: {@Model}", model);

        //        // Validate the model
        //        if (!ModelState.IsValid)
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        //            _logger.LogWarning("Model validation failed: {@Errors}", errors);
        //            return BadRequest(ModelState); // Return validation errors
        //        }

        //        // Get the user ID from the session
        //        var userId = GetUserIdFromSession();
        //        if (userId == null)
        //        {
        //            return Unauthorized("You must be logged in to make a reservation.");
        //        }

        //        // Map the ViewModel to the TaxiReservations entity
        //        var reservation = _mapper.Map<TaxiReservations>(model);
        //        reservation.UserId = userId.Value; // Set the user ID
        //        reservation.ReservationTime = DateTime.Now; // Set the reservation time

        //        // Find the taxi by ID
        //        var taxi = _context.Taxis.FirstOrDefault(t => t.TaxiId == model.TaxiId);
        //        //    if (taxi == null || taxi.Status != TaxiStatus.Available)
        //        //    {
        //        //        return BadRequest("The selected taxi is no longer available.");
        //        //    }

        //        // Update taxi status to reserved
        //        //    taxi.Status = TaxiStatus.Busy;
        //        reservation.Taxi = taxi; // Associate the taxi with the reservation

        //        // Save the reservation to the database
        //        _context.TaxiReservations.Add(reservation);
        //        _context.SaveChanges();

        //        // Log successful reservation
        //        _logger.LogInformation("User  {User Id} made a reservation for Taxi {TaxiId}.", userId.Value, taxi.TaxiId);

        //        // Return the reservation ID in the response
        //        return Ok(new { reservationId = reservation.ReservationId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while making a reservation.");
        //        return StatusCode(500, "An internal server error occurred. Please try again later.");
        //    }
        //}

        //private int? GetUserIdFromSession()
        //{
        //    // Retrieve the user ID from the session
        //    var userIdString = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        //    return int.TryParse(userIdString, out var userId) ? userId : (int?)null;
        //}

        //// GET: Reservation Confirmation
        //public IActionResult Confirmation(int id)
        //{
        //    var reservation = _context.TaxiReservations
        //        .FirstOrDefault(r => r.ReservationId == id);

        //    if (reservation == null)
        //        return NotFound();

        //    var viewModel = _mapper.Map<TaxiReservationViewModel>(reservation);
        //    return View(viewModel);
        //}
    }
}