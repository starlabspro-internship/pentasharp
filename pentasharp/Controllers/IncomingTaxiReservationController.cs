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
using pentasharp.Migrations;

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
        [HttpPost("Search")]
        public IActionResult Search([FromBody] TaxiSearchViewModel searchModel)
        {
            _logger.LogInformation("Received SearchModel: {@searchModel}", searchModel);
            var availableTaxis = _context.TaxiCompanies.ToList();

            var taxiResults = availableTaxis.Select(tc => new
            {
                companyName = tc.CompanyName,
                contactInfo = tc.ContactInfo,
                taxiCompanyId = tc.TaxiCompanyId
            }).ToList();

            _logger.LogInformation("Taxi Company Results: {@TaxiResults}", taxiResults);
            return Json(taxiResults);
        }
        [HttpPost("Confirm")]
        public IActionResult Confirm([FromBody] TaxiReservationViewModel confirmModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid reservation details.");
            }

            // Save reservation to database
            var reservation = new TaxiReservations
            {
                PickupLocation = confirmModel.PickupLocation,
                DropoffLocation = confirmModel.DropoffLocation,
                ReservationTime = confirmModel.ReservationTime,
                PassengerCount = confirmModel.PassengerCount,
                TaxiCompanyId = confirmModel.TaxiCompanyId,
                TripStartTime = confirmModel.TripStartTime,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaxiReservations.Add(reservation);
            _context.SaveChanges();

            return Ok(new { Message = "Reservation confirmed successfully!" });
        }

    }
}