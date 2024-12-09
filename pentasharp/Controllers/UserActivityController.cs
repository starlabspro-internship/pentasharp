using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pentasharp.Interfaces;
using pentasharp.Models.TaxiRequest;
using System;
using System.Threading.Tasks;
using WebApplication1.Filters;

namespace pentasharp.Controllers
{
    public class UserActivityController : Controller
    {
        private readonly ITaxiReservationService _taxiReservationService;
        private readonly ILogger<UserActivityController> _logger;

        // Change ILogger to ILogger<UserActivityController>
        public UserActivityController(ITaxiReservationService taxiReservationService, ILogger<UserActivityController> logger)
        {
            _taxiReservationService = taxiReservationService;
            _logger = logger;
        }

        [ServiceFilter(typeof(LoginRequiredFilter))]
        public async Task<IActionResult> MyReservations()
        {
            _logger.LogInformation("Fetching reservations for the user.");

            if (HttpContext.Items["UserId"] is not int userId)
            {
                return BadRequest(new { success = false, message = "User is not authenticated." });
            }

            var reservations = await _taxiReservationService.GetReservationsForUserAsync(userId);
            return View("MyReservations", reservations);
        }
    }
}
